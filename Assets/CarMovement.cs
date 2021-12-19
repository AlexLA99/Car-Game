using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    private bool isBreaking;

    [System.Serializable]
    public struct CarInfo
    {
        public int carId;
        public WheelCollider frontLeftWheelCollider;
        public WheelCollider frontRightWheelCollider;
        public WheelCollider rearLeftWheelCollider;
        public WheelCollider rearRightWheelCollider;
        public Transform frontLeftWheelTransform;
        public Transform frontRightWheelTransform;
        public Transform rearLeftWheelTransform;
        public Transform rearRightWheelTransform;
    };

    public CarInfo carInfo;
    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 0f;

    void Update()
    {
        if (transform.rotation.eulerAngles.z <= 350 && transform.rotation.eulerAngles.z >= 10)
        {
            if (Input.GetKey("space"))
            {
                transform.Rotate(0, 0, -transform.rotation.eulerAngles.z);
            }
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        carInfo.frontLeftWheelCollider.steerAngle = steerAngle;
        carInfo.frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        carInfo.frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        carInfo.frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        brakeForce = isBreaking ? 3000f : 0f;
        carInfo.frontLeftWheelCollider.brakeTorque = brakeForce;
        carInfo.frontRightWheelCollider.brakeTorque = brakeForce;
        carInfo.rearLeftWheelCollider.brakeTorque = brakeForce;
        carInfo.rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(carInfo.frontLeftWheelCollider, carInfo.frontLeftWheelTransform);
        UpdateWheelPos(carInfo.frontRightWheelCollider, carInfo.frontRightWheelTransform);
        UpdateWheelPos(carInfo.rearLeftWheelCollider, carInfo.rearLeftWheelTransform);
        UpdateWheelPos(carInfo.rearRightWheelCollider, carInfo.rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

}