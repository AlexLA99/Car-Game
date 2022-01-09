using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPointTrigger : MonoBehaviour
{
    public GameObject LapCompleteTrig;
    public GameObject HalfLapTrig;
    public GameObject FirstLapTrigger;
    public GameObject LastLapTrigger;
    public GameObject car;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == car.name)
        { 
        LapCompleteTrig.SetActive(true);
        HalfLapTrig.SetActive(true);
        FirstLapTrigger.SetActive(false);
        LastLapTrigger.SetActive(true);
        }
    }
}
