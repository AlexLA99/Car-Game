using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Assets.Networking;

public class Server_Test : MonoBehaviour
{
    private Socket serverSocket;
    private Thread listeningThread;
    public Text OutputLog;
    private object logLock = new object();
    string newText;
    bool textToLog = false;
    int playerID = 1;

    List<EndPoint> clients = new List<EndPoint>();

    // Start is called before the first frame update
    void Start()
    {
        OutputLog.text = "";
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        serverSocket.Bind(ipep);
        listeningThread = new Thread(Listening);
        listeningThread.IsBackground = true;
        listeningThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        lock (logLock)
        {
            if (textToLog)
            {
                OutputLog.text += newText;
                newText = "";
                textToLog = false;
            }
        }
    }

    void OnDestroy()
    {
        SendCloseMessage();
        serverSocket.Shutdown(SocketShutdown.Both);
        serverSocket.Close();
        listeningThread.Join();
        Debug.Log("Bye :)");
    }
    private void Listening()
    {
        Debug.Log("Starting");
        ThreadLogText("Starting");
        EndPoint clientAddr = new IPEndPoint(IPAddress.None, 0);

        Message received = new Message();
        while (true)
        {
            try
            {
                byte[] data = new byte[1024];
                int reciv = serverSocket.ReceiveFrom(data, ref clientAddr);
                received.Deserialize(data);
                //Debug.Log("Recieved " + received.message);
                //ThreadLogText("Recieved " + received.message);
                if (received.message == "Ping")
                {
                    Debug.Log("Sending Pong back" + playerID);
                    ThreadLogText("Sending Pong back" + playerID);
                    Message pongMessage = new Message();
                    pongMessage.playerId = playerID;
                    pongMessage.message = "Pong";
                    data = pongMessage.Serialize();
                    serverSocket.SendTo(data, data.Length, SocketFlags.None, clientAddr);
                    clients.Add(clientAddr);
                    ++playerID;
                }
                else if (received.message == "exit")
                {
                    Debug.Log("Exit Message recieved");
                    ThreadLogText("Exit Message received");
                    break;
                }
                else
                {
                    foreach (EndPoint client in clients)
                    {
                        serverSocket.SendTo(data, data.Length, SocketFlags.None, client);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Exception caught: " + e.GetType());
                Debug.LogException(e);
            }
        }
        Debug.Log("Exiting listening thread");
        ThreadLogText("Exiting listening thread");
    }

    private void SendCloseMessage()
    {
        Socket closeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        Message exitMessage = new Message();
        exitMessage.playerId = playerID;
        exitMessage.message = "exit";
        byte[] data = exitMessage.Serialize();
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        EndPoint remote = (EndPoint)ipep;
        closeSocket.SendTo(data, remote);
        closeSocket.Shutdown(SocketShutdown.Both);
        closeSocket.Close();
    }

    private void ThreadLogText(string text)
    {
        lock (logLock)
        {
            textToLog = true;
            newText += System.Environment.NewLine + text;
        }
    }

    byte[] getBytes(CarMovement.CarInfo str)
    {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
}
