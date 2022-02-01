using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform cam = null;

    Vector3 verticalVelocity = Vector3.zero;    
    
    private PlayerInput pI;
    private CharacterController controller;
    
    Vector3 currentVelocity;
    float gravity = -9.81f;
    float horizontalMovement = 0;
    float verticalMovement = 0;
    public float speed = 0;
    bool isGrounded;
    bool isMoving = false;

    float iTimer;
    float iTimerMax = 2;
    bool useTimer;


    private void Awake()
    {

    }

    private void Start()
    {
        pI = this.gameObject.GetComponent<PlayerInput>();
        controller = this.gameObject.GetComponent<CharacterController>();
    }

    //public void Test(object sender, EventArgs e)
    //{

    //}

    void Update()
    {
        iTimer = iTimer - Time.deltaTime;
        HandleMovement();
        //Debug.Log(speed);
    }
    
    private void HandleMovement()
    {

        if (pI.GetHorizontalMovement() == 0 && pI.GetVerticalMovement() == 0)
        {
            if (isMoving == true)
            {
                speed = Mathf.Clamp(speed - pI.pD.inertiaVar * 2f, 0, pI.pD.moveSpeed);
            }
            else
                speed = 0;
        }
        else if (speed == 0)
        {
            speed = Mathf.Clamp(speed + pI.pD.inertiaVar, 0, pI.pD.moveSpeed);
            isMoving = false;
        }
        else
        {
            speed = Mathf.Clamp(speed + pI.pD.inertiaVar * 1.1f, 0, pI.pD.moveSpeed);
            isMoving = true;
            horizontalMovement = pI.GetHorizontalMovement() / 1.5f;
            verticalMovement = pI.GetVerticalMovement();
        }

        if (verticalMovement != 0)
        {
            horizontalMovement = horizontalMovement / 2;
        }

        Vector3 movementVelocity = transform.right * horizontalMovement + transform.forward * verticalMovement;
        controller.Move(movementVelocity * speed * Time.deltaTime);
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

        //if (pI.GetHorizontalMovement() == 0 && pI.GetVerticalMovement() == 0)//
        //{
        //    if (isMoving == true)
        //    {
        //        speed = Mathf.Clamp(speed - pI.pD.inertiaVar * 2f, 0, pI.pD.moveSpeed);
        //    }
        //    else
        //        speed = 0;
        //}
        //else if (speed == 0)
        //{
        //    speed = Mathf.Clamp(speed + pI.pD.inertiaVar, 0, pI.pD.moveSpeed);
        //    isMoving = false;
        //}
        //else if (horizontalMovement != pI.GetHorizontalMovement() || verticalMovement != pI.GetVerticalMovement())
        //{
        //    speed = Mathf.Clamp(speed - pI.pD.inertiaVar * 2f, 0, pI.pD.moveSpeed);
        //}
        //else
        //{
        //    horizontalMovement = pI.GetHorizontalMovement();
        //    verticalMovement = pI.GetVerticalMovement();
        //    isMoving = true;
        //    speed = Mathf.Clamp(speed + pI.pD.inertiaVar * 1.1f, 0, pI.pD.moveSpeed);
        //}


        //Vector3 movementVelocity = transform.right * horizontalMovement + transform.forward * verticalMovement;
        //controller.Move(movementVelocity * speed * Time.deltaTime);
        //verticalVelocity.y += gravity * Time.deltaTime;
        //controller.Move(verticalVelocity * Time.deltaTime);
    }





}
