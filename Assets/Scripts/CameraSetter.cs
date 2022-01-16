using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraSetter : MonoBehaviour
{
    public GameObject lapComplete;
    public GameObject First;
    public GameObject Half;
    public GameObject Last;

    public void SetCar(int id)
    {
        GameObject car;
        if (id == 1)
        {
            car = GameObject.Find("SportCar1(Clone)");
           
        }
        else
        {
            car = GameObject.Find("SportCar2(Clone)");
        }
        GetComponent<AutoCam>().SetTarget(car.transform);
        //GetComponent<RaceFinish>().car = car.GetComponent<CarMovement>();
        //GetComponent<RaceFinish>().CarControls = car;
        CountDown countDown = GetComponentInChildren<CountDown>(true);
        //countDown.CarControls = car;
        countDown.car = car.GetComponent<CarMovement>();
        GetComponentInChildren<Speedometer>().target = car.GetComponent<Rigidbody>();
        lapComplete.GetComponent<LapComplete>().car = car;
        First.GetComponent<FirstPointTrigger>().car = car;
        Half.GetComponent<HalfPointTrigger>().car = car;
        Last.GetComponent<LastPointTrigger>().car = car;
    }
}
