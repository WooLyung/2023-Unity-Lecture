using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

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

    private IPAddress ip = IPAddress.Parse("13.124.53.124");
    private int port = 56164;
    private Socket socket = null;

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
            string message = $"{{\"evt\":\"join\",\"nickname\":\"{nickname}\",\"color\":\"{color}\"}}#";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            socket.Send(messageBytes);

            byte[] buffer = new byte[1024];
            int bytesReceived = socket.Receive(buffer);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
            SocketResult result = JsonUtility.FromJson<SocketResult>(response);

            if (result.result == "success")
                return true;
            else
                return false;
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
                byte[] buffer = new byte[1024];
                int bytesReceived = socket.Receive(buffer);
                string[] response = Encoding.ASCII.GetString(buffer, 0, bytesReceived).Split("#");
                foreach (string res in response)
                {
                    if (JsonUtility.FromJson<OnEvent_Update>(res) != null)
                        on_queue.Enqueue(JsonUtility.FromJson<OnEvent_Update>(res));
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}