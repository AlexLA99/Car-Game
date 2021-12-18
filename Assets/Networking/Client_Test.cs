using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine.UI;

public class Client_Test : MonoBehaviour
{
    private Socket connectionSocket;
    private Thread listeningThread;
    private object quitLock = new object();
    bool quit = false;
    public Text OutputLog;
    private object logLock = new object();
    string newText;
    bool textToLog = false;

    // Start is called before the first frame update
    void Start()
    {
        OutputLog.text = "";
        connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9051);
        connectionSocket.Bind(ipep);
        listeningThread = new Thread(Connection);
        listeningThread.IsBackground = true;
        listeningThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        bool quitting = false;
        lock (quitLock)
        {
            quitting = quit;
        }
        if (quitting)
        {
            Application.Quit();
        }
        
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
        listeningThread.Join();
        connectionSocket.Shutdown(SocketShutdown.Both);
        connectionSocket.Close();
        Debug.Log("Bye :)");
    }
    private void Connection()
    {
        IPEndPoint serverIpep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remote = (EndPoint)ipep;

        Debug.Log("Sending first ping");
        ThreadLogText("Sending first ping");
        System.DateTime localDate = System.DateTime.Now;
        byte[] data = Encoding.ASCII.GetBytes("Ping");
        connectionSocket.SendTo(data, data.Length, SocketFlags.None, serverIpep);
        while (true)
        {
            try
            {
                data = new byte[1024];
                int reciv = connectionSocket.ReceiveFrom(data, ref remote);
                string recieved = Encoding.ASCII.GetString(data, 0, reciv);
                Debug.Log("Recieved " + recieved);
                ThreadLogText("Recieved " + recieved);
                System.DateTime localDate2 = System.DateTime.Now;
                ThreadLogText("Client-Server ping: " + (localDate2.Millisecond - localDate.Millisecond).ToString());
                if (recieved == "Pong")
                {
                    // Check time it takes to receive
                    Debug.Log("Connection Established with a delay of " + (localDate2.Millisecond - localDate.Millisecond).ToString() + " milliseconds");
                    ThreadLogText("Connection Established");
                    data = Encoding.ASCII.GetBytes("Nice");
                    connectionSocket.SendTo(data, data.Length, SocketFlags.None, remote);
                }
                else if (recieved == "exit")
                {
                    Debug.Log("Exit Message recieved");
                    ThreadLogText("Exit Message recieved");
                    break;
                }
            }
            catch (SocketException e)
            {
                Debug.Log("Exception caught: " + e.GetType());
                Debug.Log(e.Message);
                if (e.SocketErrorCode == SocketError.ConnectionReset)
                {
                    Debug.Log("Could not connect to server because it is down");
                    Debug.Log("Exiting client app");
                    ThreadLogText("Could not connect to server because it is down");
                    ThreadLogText("Exiting client app");
                    lock (quitLock)
                    {
                        quit = true;
                    }
                }
            }
        }
        Debug.Log("Exiting listening thread");
        ThreadLogText("Exiting listening thread");
    }

    private void SendCloseMessage()
    {
        Socket closeSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        byte[] data = new byte[1024];
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9051);
        EndPoint remote = (EndPoint)ipep;
        data = Encoding.ASCII.GetBytes("exit");
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
}