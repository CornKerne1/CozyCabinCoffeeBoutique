using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CarController : MonoBehaviour
{
    
    public enum Axel
    {
        Front,
        Rear
    }
    public enum DriveType
    {
        FrontWheel,
        RearWheel
    }

   [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }
    
    [SerializeField] private DriveType carType;
    
    [SerializeField] private float motorForce = 50.0f;

    [SerializeField] private float turnSensitivity = 1.0f;
    [SerializeField]private float maxSteerAngle = 30.0f;

    [SerializeField]private Vector3 centerOfMass;

    [SerializeField]private List<Wheel> wheels;

    [SerializeField]private PlayerInput playerInput;
    
    private Rigidbody _carRb;


    void Start()
    {
        _carRb = GetComponent<Rigidbody>();
        _carRb.centerOfMass = centerOfMass;
    }
    void FixedUpdate()
    {
        SteerAndAccelerate();
    }


    void SteerAndAccelerate()
    {
        foreach(var wheel in wheels)
        {
            var steerAngle = 0f;
            switch (carType)
            {
                case DriveType.FrontWheel:
                    if(wheel.axel == Axel.Front)
                    {
                        steerAngle = playerInput.GetHorizontalMovement() * turnSensitivity * maxSteerAngle;
                        wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                        if (wheel.wheelCollider.isGrounded)
                            wheel.wheelCollider.motorTorque = playerInput.GetVerticalMovement() * motorForce;
                        else
                            wheel.wheelCollider.motorTorque = wheel.wheelCollider.motorTorque / 1.1f;
                    }
                    UpdateTireMesh(wheel.wheelCollider,wheel.wheelModel.transform);
                    break;
                case DriveType.RearWheel:
                    if(wheel.axel == Axel.Rear)
                    {
                        steerAngle = -playerInput.GetHorizontalMovement() * turnSensitivity * maxSteerAngle;
                        wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                        wheel.wheelCollider.motorTorque = playerInput.GetVerticalMovement() * motorForce;
                        if (wheel.wheelCollider.isGrounded)
                            wheel.wheelCollider.motorTorque = playerInput.GetVerticalMovement() * motorForce;
                        else
                            wheel.wheelCollider.motorTorque = wheel.wheelCollider.motorTorque / 1.01f;
                    }
                    UpdateTireMesh(wheel.wheelCollider,wheel.wheelModel.transform);
                    break;
            }
        }
    }
    private void UpdateTireMesh(WheelCollider collider, Transform tireTransform)
    {
        Vector3 _pos = tireTransform.position;
        Quaternion _quat = tireTransform.rotation;
        collider.GetWorldPose(out _pos,out _quat);

        tireTransform.position = _pos;
        tireTransform.rotation = _quat;
    }
    
}

