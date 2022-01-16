using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CarSpawner : MonoBehaviour
{
    public CameraSetter CameraSetter;

    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void SpawnCar(int playerNumber, CarMovement[] Cars)
    {
        GameObject sp1 = GameObject.Find("Spawn1");
        GameObject sp2 = GameObject.Find("Spawn2");

        switch (playerNumber)
        {
            case 1:
                PhotonNetwork.Instantiate("PhotonPrefabs/Car/SportCar1", sp1.transform.position, sp1.transform.rotation);
                break;
            case 2:
                PhotonNetwork.Instantiate("PhotonPrefabs/Car/SportCar2", sp2.transform.position, sp2.transform.rotation);
                break;


        }
        CameraSetter.SetCar(Cars[PhotonNetwork.LocalPlayer.ActorNumber - 1].gameObject, PhotonNetwork.LocalPlayer.ActorNumber);
    }
}
