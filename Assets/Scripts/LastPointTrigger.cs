using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPointTrigger : MonoBehaviour
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
            HalfLapTrig.SetActive(false);
            FirstLapTrigger.SetActive(false);
            LastLapTrigger.SetActive(false);
        }
    }
}
