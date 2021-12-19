//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Laps : MonoBehaviour
//{
//    public GameObject LapCompleteTrig;
//    public GameObject HalfLapTrig;

//    public GameObject MinuteDisplay;
//    public GameObject SecondDisplay;
//    public GameObject MilliDisplay;

//    public GameObject LapTimeBox;

//    public GameObject LapCounter;
//    public int LapsDone;

//    void OnTriggerEnter()
//    {
//        LapsDone += 1;
//        if (LapTimeManager.SecondCount <= 9)
//        {
//            SecondDisplay.GetComponent<Text>().text = "0" + LapTimeManager.SecondCount + ".";
//        }
//        else
//        {
//            SecondDisplay.GetComponent<Text>().text = "" + LapTimeManager.SecondCount + ".";
//        }

//        if (LapTimeManager.MinuteCount <= 9)
//        {
//            MinuteDisplay.GetComponent<Text>().text = "0" + LapTimeManager.MinuteCount + ".";
//        }
//        else
//        {
//            MinuteDisplay.GetComponent<Text>().text = "" + LapTimeManager.MinuteCount + ".";
//        }

//        MilliDisplay.GetComponent<text>().text = "" + LapTimeManager.MilliCount;

//        PlayerPrefs.SetInt("MinSave", LapTimeManager.MinuteCount);
//        PlayerPrefs.SetInt("SecSave", LapTimeManager.SecondCount);
//        PlayerPrefs.SetInt("MilliSave", LapTimeManager.MilliCount);

//        LapTimeManager.MinuteCount = 0;
//        LapTimeManager.SecondCount = 0;
//        LapTimeManager.MilliCount = 0;
//        LapCounter.GetComponent<text>().text = "" + LapsDone;

//        HalfLapTrig.SetActive(true);
//        LapCompleteTrig.SetActive(true);
//    }

//}
