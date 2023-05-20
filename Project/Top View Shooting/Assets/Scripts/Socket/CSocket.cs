using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class CSocket
{
    private static CSocket instance = null;
    public static CSocket Instance {  get
        {
            if (instance == null)
                instance = new CSocket();
            return instance;
        }
    }

    private IPAddress ip = IPAddress.Parse("34.64.40.5"); // 고정된 IP
    private int port = 9172; // 고정된 PORT
    private Socket socket = null;
    private string buffer = ""; // 임시 데이터 저장 공간

    public OnEvent_Init Init()
    {
        string response = Read();
        return JsonUtility.FromJson<OnEvent_Init>(response);
    }

    private string Read() // 서버로부터 온전한 데이터 하나를 읽어서 반환
    {
        try
        {
            while (true)
            {
                if (!buffer.Contains("#")) // 버퍼에 온전한 데이터가 없다면 추가적으로 데이터를 읽음
                {
                    byte[] byteBuffer = new byte[1024];
                    int byteReceived = socket.Receive(byteBuffer);
                    string response = Encoding.ASCII.GetString(byteBuffer, 0, byteReceived);
                    buffer += response;
                }
                if (buffer.Contains("#")) // 버퍼에 온전한 데이터가 있다면 그 중 가장 처음에 있는 데이터를 잘라서 반환
                {
                    string[] splits = buffer.Split("#");
                    buffer = splits[1];
                    for (int i = 2; i < splits.Length; i++)
                        buffer += "#" + splits[i];
                    return splits[0];
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return "";
        }
    }

    public bool Connect()
    {
        if (socket == null)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(ip, port); // ip:port
                socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true); // 속도 향상
                socket.Connect(remoteEP);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                socket = null;
                return false;
            }
        }
        return true;
    }

    public bool Join(string nickname, string color)
    {
        try
        {
            EmitEvent(new EmitEvent_Join(nickname, color));
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    private ConcurrentQueue<OnEvent> on_queue = new ConcurrentQueue<OnEvent>();

    public OnEvent Dequeue_on()
    {
        OnEvent evt;
        on_queue.TryDequeue(out evt);
        return evt;
    }

    public void Run()
    {

        Thread t = new Thread(new ThreadStart(ReadThread));
        t.Start();
    }

    private void ReadThread()
    {
        while (true)
        {
            try
            {
                string response = Read();
                if (JsonUtility.FromJson<OnEvent_Update>(response) != null)
                    on_queue.Enqueue(JsonUtility.FromJson<OnEvent_Update>(response));
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void EmitEvent(EmitEvent evt)
    {
        string message = JsonUtility.ToJson(evt) + "#";
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        socket.Send(messageBytes);
    }
}