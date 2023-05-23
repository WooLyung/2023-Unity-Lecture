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

    // private IPAddress ip = IPAddress.Parse("34.64.40.5");
    private IPAddress ip = IPAddress.Parse("127.0.0.1");
    private int port = 9172;
    private Socket socket = null;

    private OnEvent Read()
    {
        try
        {
            byte[] intBuffer = new byte[4];
            int byteReceived;

            byteReceived = socket.Receive(intBuffer);
            int size = BitConverter.ToInt32(intBuffer);
            byteReceived = socket.Receive(intBuffer);
            int code = BitConverter.ToInt32(intBuffer);

            byte[] buffer = new byte[1024];
            byte[] convertBuffer = new byte[size];
            int sumByte = 0;

            while (sumByte < size)
            {
                byteReceived = socket.Receive(buffer);
                Array.Copy(buffer, 0, convertBuffer, sumByte, byteReceived);
                sumByte += byteReceived;
            }

            if (code == 0) // init
                return null;
            if (code == 1) // update
                return null;

            return null;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
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
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                socket.ReceiveBufferSize = 8192;
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
            EmitEvent("join", new EmitEvent_Join(nickname, color));
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
        return Read() as OnEvent_Init;
    }

    private ConcurrentQueue<OnEvent> on_queue = new ConcurrentQueue<OnEvent>();

    public OnEvent Dequeue_on()
    {
        OnEvent evt;
        on_queue.TryDequeue(out evt);
        return evt;
    }

    public void EmitEvent(string name, EmitEvent evt)
    {
        int size = 0, offset = 8;

        List<byte[]> byteArray = evt.ToBinary();
        foreach (byte[] bytes in byteArray)
            size += bytes.Length;

        byte[] buffer = new byte[size + 8];
        BitConverter.GetBytes(size).CopyTo(buffer, 0);
        BitConverter.GetBytes(evt.GetCode()).CopyTo(buffer, 4);

        foreach (byte[] bytes in byteArray)
        {
            bytes.CopyTo(buffer, offset);
            offset += bytes.Length;
        }

        socket.Send(buffer);
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