using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading;
using Assets.Networking;
using System;

public class GameManager : MonoBehaviour
{
    PhotonView PV;
    
    public CarMovement[] Cars = new CarMovement[2];
    public CarMovement.PassInfo car1Info;
    public CarMovement.PassInfo car2Info;



    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(Cars[PhotonNetwork.LocalPlayer.ActorNumber - 1].data);

        else if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            Cars[1].serverData = (byte[])stream.ReceiveNext();
        else
            Cars[0].serverData = (byte[])stream.ReceiveNext();
    }

    private void CreateController()
    {
        int i = PhotonNetwork.LocalPlayer.ActorNumber;

        GameObject spawn = GameObject.Find("Spawn1");

        switch (i)
        {
            case 1:
                spawn = GameObject.Find("Spawn1");
                break;
            case 2:
                spawn = GameObject.Find("Spawn2");
                break;
        }

        spawn.gameObject.GetComponent<CarSpawner>().SpawnCar(i);
    }
}
