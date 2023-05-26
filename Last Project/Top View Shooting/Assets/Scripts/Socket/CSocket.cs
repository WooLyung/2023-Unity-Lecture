using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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
    // private IPAddress ip = IPAddress.Parse("127.0.0.1");
    private int port = 9172;
    private Socket socket = null;
    public bool isRunning { private set; get; } = true;

    private OnEvent Read()
    {
        try
        {
            byte[] intBuffer = new byte[4];
            int byteReceived;

            byteReceived = socket.Receive(intBuffer);
            if (byteReceived == 0)
            {
                Disconnect();
                return null;
            }

            int size = BitConverter.ToInt32(intBuffer);
            byteReceived = socket.Receive(intBuffer);
            int code = BitConverter.ToInt32(intBuffer);

            byte[] buffer = new byte[1024];
            byte[] dataBuffer = new byte[size];
            int sumByte = 0;

            while (sumByte < size)
            {
                byteReceived = socket.Receive(buffer, Mathf.Min(1024, size - sumByte), SocketFlags.None);
                Array.Copy(buffer, 0, dataBuffer, sumByte, byteReceived);
                sumByte += byteReceived;
            }

            if (code == 0) // init
                return new OnEvent_Init(dataBuffer);
            if (code == 1) // update
                return new OnEvent_Update(dataBuffer);
            if (code == 2) // join
                return new OnEvent_Join(dataBuffer);
            if (code == 3) // leave
                return new OnEvent_Leave(dataBuffer);

            return null;
        }
        catch (SocketException e)
        {
            if (e.SocketErrorCode == SocketError.TimedOut)
            {
                Disconnect();
                return null;
            }
            Debug.Log(e);
            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private void Disconnect()
    {
        isRunning = false;
        if (!socket.Connected)
            socket.Disconnect(true);
        Debug.Log("Disconnect");
    }

    public bool Connect()
    {
        if (socket == null)
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(ip, port);
                socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                socket.ReceiveBufferSize = 8192;
                socket.ReceiveTimeout = 1000;
                socket.SendTimeout = 1000;
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
        OnEvent evt = null;
        while (!((evt = Read()) is OnEvent_Init));
        return evt as OnEvent_Init;
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
        if (socket.Connected)
            socket.Send(evt.ToBinary());
    }

    public void Run()
    {
        Thread t = new Thread(new ThreadStart(ReadThread));
        t.Start();
    }

    private void ReadThread()
    {
        while (isRunning)
        {
            try
            {
                if (!socket.Connected)
                {
                    Disconnect();
                    break;
                }
                OnEvent response = Read();
                on_queue.Enqueue(response);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}