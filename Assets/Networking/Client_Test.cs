using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine.UI;
using Assets.Networking;

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
    public CarMovement[] Cars = new CarMovement[2];
    public CarMovement.PassInfo car1Info;
    public CarMovement.PassInfo car2Info;
    public CameraSetter CameraSetter;

    public GameObject countDownScript;
    public GameObject waitingPlayers;
    public GameObject countDownUI;

    bool firstTime = true;
    bool ready = false;

    bool needsConnect = false;

    [HideInInspector]
    public int playerId;

    // Start is called before the first frame update
    void Start()
    {
        //OutputLog.text = "";
        connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 0);
        connectionSocket.Bind(ipep);
        listeningThread = new Thread(Connection);
        listeningThread.IsBackground = true;
        listeningThread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (needsConnect)
        {
            CameraSetter.SetCar(Cars[playerId - 1].gameObject, 1);
            needsConnect = false;
        }
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
            if (textToLog && OutputLog != null)
            {
                OutputLog.text += newText;
                newText = "";
                textToLog = false;
            }
        }

        if (firstTime && ready)
        {
            firstTime = false;
            waitingPlayers.SetActive(false);
            countDownScript.SetActive(true);
            countDownUI.SetActive(true);

        }
    }

    void OnDestroy()
    {
        SendCloseMessage();
        connectionSocket.Shutdown(SocketShutdown.Both);
        connectionSocket.Close();
        listeningThread.Join();
        Debug.Log("Bye :)");
    }
    private void Connection()
    {
        EndPoint serverIpep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 0);
        EndPoint remote = (EndPoint)ipep;

        Debug.Log("Sending first ping");
        //ThreadLogText("Sending first ping");
        System.DateTime localDate = System.DateTime.Now;
        Message pingMessage = new Message();
        pingMessage.message = "Ping";
        byte[] data = pingMessage.Serialize();
        connectionSocket.SendTo(data, data.Length, SocketFlags.None, serverIpep);
        while (true)
        {
            try
            {
                data = new byte[1024];
                int reciv = connectionSocket.ReceiveFrom(data, ref serverIpep);
                Message message = new Message();
                message.Deserialize(data);
                //int recieved = BitConverter.ToInt32(data, 0);
                //string recieved2 = Encoding.ASCII.GetString(data, 0, reciv);
                //Debug.Log("Recieved " + message.message);
                //ThreadLogText("Recieved " + recieved);
                System.DateTime localDate2 = System.DateTime.Now;
                //ThreadLogText("Client-Server ping: " + (localDate2.Millisecond - localDate.Millisecond).ToString());
                

                //if (recieved == "exit")
                //{
                //    Debug.Log("Exit Message recieved");
                //    ThreadLogText("Exit Message recieved");
                //    break;
                //}
                //else
                //{
                // Check time it takes to receive
                if (message.message == "Pong")
                {
                    playerId = message.playerId;
                    Debug.Log("Connection Established with a delay of " + (localDate2.Millisecond - localDate.Millisecond).ToString() + " milliseconds");
                    ThreadLogText("Connection Established");
                    needsConnect = true;

                    data = Cars[playerId - 1].data;

                    Message toSend = new Message();
                    toSend.playerId = playerId;
                    toSend.payload = data;
                    data = toSend.Serialize();
                    connectionSocket.SendTo(data, data.Length, SocketFlags.None, serverIpep);
                    
                }
                else if (message.message == "exit")
                {
                    Debug.Log("Exit Message recieved");
                    ThreadLogText("Exit Message recieved");
                    break;
                }
                else if (message.message == "ready")
                {
                    ready = true;
                    Cars[message.playerId - 1].serverData = message.payload;
                    data = Cars[playerId - 1].data;

                    Message toSend = new Message();
                    toSend.playerId = playerId;
                    toSend.payload = data;
                    data = toSend.Serialize();
                    connectionSocket.SendTo(data, data.Length, SocketFlags.None, serverIpep);
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