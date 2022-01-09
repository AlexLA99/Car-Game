using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RaceFinish : MonoBehaviour
{
    public CarMovement car;
    public GameObject CarControls;
    public GameObject FinishCam;
    public GameObject CompleteTrig;


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
