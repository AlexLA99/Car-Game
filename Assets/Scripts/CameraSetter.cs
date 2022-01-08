using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraSetter : MonoBehaviour
{
    public void SetCar(GameObject car)
    {
        GetComponent<AutoCam>().SetTarget(car.transform);
        //GetComponent<RaceFinish>().car = car.GetComponent<CarMovement>();
        //GetComponent<RaceFinish>().CarControls = car;
        CountDown countDown = GetComponentInChildren<CountDown>(true);
        countDown.CarControls = car;
        countDown.car = car.GetComponent<CarMovement>();
        GetComponentInChildren<Speedometer>().target = car.GetComponent<Rigidbody>();
    }
}
