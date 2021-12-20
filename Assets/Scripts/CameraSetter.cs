using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraSetter : MonoBehaviour
{
    public void SetCar(GameObject car)
    {
        GetComponent<AutoCam>().SetTarget(car.transform);

        CountDown countDown = GetComponentInChildren<CountDown>();
        countDown.CarControls = car;
        countDown.car = car.GetComponent<CarMovement>();
        GetComponentInChildren<Speedometer>().target = car.GetComponent<Rigidbody>();
    }
}
