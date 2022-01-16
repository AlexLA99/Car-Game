using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading;
using Assets.Networking;

public class GameManager : MonoBehaviour
{
    public CameraSetter CameraSetter;
    public CarMovement[] Cars = new CarMovement[2];
    public CarMovement.PassInfo car1Info;
    public CarMovement.PassInfo car2Info;

    public int playerId = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        playerId = PhotonNetwork.LocalPlayer.ActorNumber;
        CameraSetter.SetCar(Cars[playerId-1].gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(Cars[playerId-1].data);

        else if (playerId == 1)
            Cars[2].serverData = (byte[])stream.ReceiveNext();
        else
            Cars[1].serverData = (byte[])stream.ReceiveNext();
    }
}
