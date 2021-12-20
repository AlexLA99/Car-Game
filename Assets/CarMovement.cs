using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CarMovement : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    

    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
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

    public struct PassInfo
    {
        public int carId;
        public float frontLeftWheelColliderSteerAngle;
        public float frontLeftWheelColliderMotorTorque;
        public float frontLeftWheelColliderBrakeTorque;

        public float frontRightWheelColliderMotorTorque;
        public float frontRightWheelColliderBrakeTorque;
        public float frontRightWheelColliderSteerAngle;

        public float backLeftWheelColliderSteerAngle;
        public float backLeftWheelColliderMotorTorque;
        public float backLeftWheelColliderBrakeTorque;
                    
        public float backRightWheelColliderMotorTorque;
        public float backRightWheelColliderBrakeTorque;
        public float backRightWheelColliderSteerAngle;

        [HideInInspector]
        public bool isBreaking;
    }

    public PassInfo passInfo;
    public CarInfo carInfo;
    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 0f;
    public Client_Test clientCar;
    
    PassInfo ClientData;

    public byte[] data;
    public byte[] serverData;

    void Update()
    {
        if (transform.rotation.eulerAngles.z <= 350 && transform.rotation.eulerAngles.z >= 10)
        {
            if (Input.GetKey("space"))
            {
                transform.Rotate(0, 0, -transform.rotation.eulerAngles.z);
            }
        }

        passInfo.frontLeftWheelColliderSteerAngle = carInfo.frontLeftWheelCollider.steerAngle;
        passInfo.frontLeftWheelColliderMotorTorque = carInfo.frontLeftWheelCollider.motorTorque;
        passInfo.frontLeftWheelColliderBrakeTorque = carInfo.frontLeftWheelCollider.brakeTorque;
        
        passInfo.frontRightWheelColliderSteerAngle = carInfo.frontRightWheelCollider.steerAngle;
        passInfo.frontRightWheelColliderMotorTorque = carInfo.frontRightWheelCollider.motorTorque;
        passInfo.frontRightWheelColliderBrakeTorque = carInfo.frontRightWheelCollider.brakeTorque;
      
        passInfo.backLeftWheelColliderSteerAngle = carInfo.rearLeftWheelCollider.steerAngle;
        passInfo.backLeftWheelColliderMotorTorque = carInfo.rearLeftWheelCollider.motorTorque;
        passInfo.backLeftWheelColliderBrakeTorque= carInfo.rearLeftWheelCollider.brakeTorque;
        
        passInfo.backRightWheelColliderSteerAngle = carInfo.rearRightWheelCollider.steerAngle;
        passInfo.backRightWheelColliderMotorTorque = carInfo.rearRightWheelCollider.motorTorque;
        passInfo.backRightWheelColliderBrakeTorque = carInfo.rearRightWheelCollider.brakeTorque;
        passInfo.carId = carInfo.carId;


        data = getBytes(passInfo);

        if (serverData != null && serverData.Length != 0)
        {
            ClientData = fromBytes(serverData);
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
        passInfo.isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleSteering()
    {
        if (clientCar.playerId == carInfo.carId)
        {
            steerAngle = maxSteeringAngle * horizontalInput;
            carInfo.frontLeftWheelCollider.steerAngle = steerAngle;
            carInfo.frontRightWheelCollider.steerAngle = steerAngle;
        }
        else if (ClientData.carId == carInfo.carId)
        {
            carInfo.frontLeftWheelCollider.steerAngle = ClientData.frontLeftWheelColliderSteerAngle;
            carInfo.frontRightWheelCollider.steerAngle = ClientData.frontRightWheelColliderSteerAngle;
        }
        
    }

    private void HandleMotor()
    {
        if (clientCar.playerId == carInfo.carId)
        {
            carInfo.frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            carInfo.frontRightWheelCollider.motorTorque = verticalInput * motorForce;

            brakeForce = passInfo.isBreaking ? 3000f : 0f;
            carInfo.frontLeftWheelCollider.brakeTorque = brakeForce;
            carInfo.frontRightWheelCollider.brakeTorque = brakeForce;
            carInfo.rearLeftWheelCollider.brakeTorque = brakeForce;
            carInfo.rearRightWheelCollider.brakeTorque = brakeForce;
        }
        else if (ClientData.carId == carInfo.carId)
        {
            carInfo.frontLeftWheelCollider.motorTorque = ClientData.frontLeftWheelColliderMotorTorque;
            carInfo.frontRightWheelCollider.motorTorque = ClientData.frontRightWheelColliderMotorTorque;

            brakeForce = ClientData.isBreaking ? 3000f : 0f;
            carInfo.frontLeftWheelCollider.brakeTorque = ClientData.frontLeftWheelColliderBrakeTorque;
            carInfo.frontRightWheelCollider.brakeTorque = ClientData.frontRightWheelColliderBrakeTorque;
            carInfo.rearLeftWheelCollider.brakeTorque = ClientData.backLeftWheelColliderBrakeTorque;
            carInfo.rearRightWheelCollider.brakeTorque = ClientData.backRightWheelColliderBrakeTorque;
        }
    }

    private void UpdateWheels()
    {
        //if (clientCar.playerId == carInfo.carId)
        {
            UpdateWheelPos(carInfo.frontLeftWheelCollider, carInfo.frontLeftWheelTransform);
            UpdateWheelPos(carInfo.frontRightWheelCollider, carInfo.frontRightWheelTransform);
            UpdateWheelPos(carInfo.rearLeftWheelCollider, carInfo.rearLeftWheelTransform);
            UpdateWheelPos(carInfo.rearRightWheelCollider, carInfo.rearRightWheelTransform);
        }
        //else
        //{
        //    UpdateWheelPos(clientCar.serverCar.frontLeftWheelCollider, clientCar.serverCar.frontLeftWheelTransform);
        //    UpdateWheelPos(clientCar.serverCar.frontRightWheelCollider, clientCar.serverCar.frontRightWheelTransform);
        //    UpdateWheelPos(clientCar.serverCar.rearLeftWheelCollider, clientCar.serverCar.rearLeftWheelTransform);
        //    UpdateWheelPos(clientCar.serverCar.rearRightWheelCollider, clientCar.serverCar.rearRightWheelTransform);
        //}
        
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    public byte[] getBytes(PassInfo str)
    {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }

    public PassInfo fromBytes(byte[] arr)
    {
        int size = Marshal.SizeOf<PassInfo>();
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        PassInfo str = Marshal.PtrToStructure<PassInfo>(ptr);
        Marshal.FreeHGlobal(ptr);

        return str;
    }

}