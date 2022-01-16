using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Photon.Pun;

public class CarMovement : MonoBehaviour
{
    
    private float steerAngle;
    private int positionCountDown = 0;

    public GameObject LapCompleteTrig;
    public GameObject HalfLapTrig;
    public GameObject FirstLapTrigger;
    public GameObject LastLapTrigger;

    public Vector3 CheckPointPos;
    public Quaternion CheckpointRot;

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
        public float horizontalInput;
        public float verticalInput;
        public float carTransformX;
        public float carTransformY;
        public float carTransformZ;
        public float carTransformRotX;
        public float carTransformRotY;
        public float carTransformRotZ;
        public float carTransformRotW;

        [HideInInspector]
        public bool isBreaking;
    }

    public PassInfo passInfo;
    public CarInfo carInfo;
    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 0f;
    public GameManager clientCar;
    
    PassInfo ClientData;

    [HideInInspector]
    public byte[] data;
    [HideInInspector]
    public byte[] serverData;


    private void Start()
    {
        passInfo.carId = carInfo.carId;
    }

    void Update()
    {

        if (PhotonNetwork.LocalPlayer.ActorNumber == carInfo.carId && Input.GetKey(KeyCode.R))
        {
            LastCheckpoint();
        }
        

        passInfo.carTransformX = transform.position.x;
        passInfo.carTransformY = transform.position.y;
        passInfo.carTransformZ = transform.position.z;

        passInfo.carTransformRotX = transform.rotation.x;
        passInfo.carTransformRotY = transform.rotation.y;
        passInfo.carTransformRotZ = transform.rotation.z;
        passInfo.carTransformRotW = transform.rotation.w;




        data = getBytes(passInfo);

        if (serverData != null && serverData.Length != 0)
        {
            ClientData = fromBytes(serverData);
        }
        ++positionCountDown;
        //if (positionCountDown >= 10 && PhotonNetwork.LocalPlayer.ActorNumber != carInfo.carId && ClientData.carId == carInfo.carId)
        //{
        //    transform.rotation = new Quaternion(ClientData.carTransformRotX, ClientData.carTransformRotY, ClientData.carTransformRotZ, ClientData.carTransformRotW);
        //    transform.position = new Vector3(ClientData.carTransformX, ClientData.carTransformY, ClientData.carTransformZ);
        //    positionCountDown = 0;
        //}
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
        if (PhotonNetwork.LocalPlayer.ActorNumber == carInfo.carId)
        {
            passInfo.horizontalInput = Input.GetAxis("Horizontal");
            passInfo.verticalInput = Input.GetAxis("Vertical");
            passInfo.isBreaking = Input.GetKey(KeyCode.Space);
        }
        else if (ClientData.carId == carInfo.carId)
        {
            passInfo.horizontalInput = ClientData.horizontalInput;
            passInfo.verticalInput = ClientData.verticalInput;
            passInfo.isBreaking = ClientData.isBreaking;
        }
        
    }

    private void HandleSteering()
    {
            steerAngle = maxSteeringAngle * passInfo.horizontalInput;
            carInfo.frontLeftWheelCollider.steerAngle = steerAngle;
            carInfo.frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
            carInfo.frontLeftWheelCollider.motorTorque = passInfo.verticalInput * motorForce;
            carInfo.frontRightWheelCollider.motorTorque = passInfo.verticalInput * motorForce;

            brakeForce = passInfo.isBreaking ? 3000f : 0f;
            carInfo.frontLeftWheelCollider.brakeTorque = brakeForce;
            carInfo.frontRightWheelCollider.brakeTorque = brakeForce;
            carInfo.rearLeftWheelCollider.brakeTorque = brakeForce;
            carInfo.rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        {
            UpdateWheelPos(carInfo.frontLeftWheelCollider, carInfo.frontLeftWheelTransform);
            UpdateWheelPos(carInfo.frontRightWheelCollider, carInfo.frontRightWheelTransform);
            UpdateWheelPos(carInfo.rearLeftWheelCollider, carInfo.rearLeftWheelTransform);
            UpdateWheelPos(carInfo.rearRightWheelCollider, carInfo.rearRightWheelTransform);
        }
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

    public void LastCheckpoint()
    {

        
            CheckPointPos = new Vector3(-28.94f, 4.0f, 114.1f);
            CheckpointRot = new Quaternion(0, -126.75f, 0, 60);
        

        transform.position = CheckPointPos;
        transform.rotation = CheckpointRot;
    }
}