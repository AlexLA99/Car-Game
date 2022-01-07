﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfPointTrigger : MonoBehaviour
{
    public GameObject LapCompleteTrig;
    public GameObject HalfLapTrig;
    public GameObject FirstLapTrigger;
    public GameObject LastLapTrigger;

    void OnTriggerEnter()
    {
        LapCompleteTrig.SetActive(true);
        HalfLapTrig.SetActive(false);
        FirstLapTrigger.SetActive(true);
        LastLapTrigger.SetActive(true);
    }
}
