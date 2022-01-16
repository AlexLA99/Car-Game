using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class RaceFinish : MonoBehaviour
{
    public CarMovement car;
    public GameObject CarControls;
    public GameObject FinishCam;
    public GameObject CompleteTrig;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            CarControls = GameObject.Find("SportCar1(Clone)");
           
        }
        
        else
        {
            CarControls = GameObject.Find("SportCar2(Clone)");
        }
        car = CarControls.GetComponent<CarMovement>();
    }
    void OnTriggerEnter()
    {
       
            this.GetComponent<BoxCollider>().enabled = false;
            CompleteTrig.SetActive(false);
            CarControls.SetActive(false);
            FinishCam.SetActive(true);
            car.motorForce = 0;
            CarControls.SetActive(true);
       
    }
}
