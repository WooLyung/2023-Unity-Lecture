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
    public static CSocket Instance { get
        {
            if (instance == null)
                instance = new CSocket();
            return instance;
        }
    }

    private IPAddress ip = IPAddress.Parse("34.64.40.5");
    private int port = 9172;
    private Socket socket = null;
    private string buffer = "";

    private string Read()
    {
        try
        {
            while (true)
            {
                byte[] byteBuffer = new byte[1024];
                int bytesReceived = socket.Receive(byteBuffer);
                string response = Encoding.ASCII.GetString(byteBuffer, 0, bytesReceived);
                buffer += response;

                if (response.Contains("#"))
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
                IPEndPoint remoteEP = new IPEndPoint(ip, port);
                socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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

    public OnEvent_Init Init()
    {
        string response = Read();
        return JsonUtility.FromJson<OnEvent_Init>(response);
    }

    private ConcurrentQueue<OnEvent> on_queue = new ConcurrentQueue<OnEvent>();

    public OnEvent Dequeue_on()
    {
        OnEvent evt;
        on_queue.TryDequeue(out evt);
        return evt;
    }

    public void EmitEvent(EmitEvent evt)
    {
        string message = JsonUtility.ToJson(evt) + "#";
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        socket.Send(messageBytes);
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
                Debug.Log(on_queue.Count);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}