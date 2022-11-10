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

   [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider WheelCollider;
        public Axel axel;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    public PlayerInput playerInput;
    
    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass;
    }
    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
      
        Steer();
    }

    void Move()
    {
        foreach(var wheel in wheels)
        {
            if (wheel.WheelCollider.isGrounded)
                wheel.WheelCollider.motorTorque = playerInput.GetVerticalMovement() * 3500 * maxAcceleration * Time.deltaTime;
            else
                wheel.WheelCollider.motorTorque = 0;
        }
    }

    void Steer()
    {
        foreach(var wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                var steerAngle = playerInput.GetHorizontalMovement() * turnSensitivity * maxSteerAngle;
                wheel.WheelCollider.steerAngle = Mathf.Lerp(wheel.WheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }
}

