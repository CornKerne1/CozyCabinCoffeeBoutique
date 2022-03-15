using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    private PlayerInput pI;
    public CharacterController controller;
    
    Vector3 velocity;
    float gravity = -9.81f;
    //float horizontalMovement = 0;
    float groundDistance = .4f;
    public bool isGrounded;
    public LayerMask groundMask;
    private Vector3 currentMovement;

    //float iTimer;
    //float iTimerMax = 2;
    //bool useTimer;


    private void Awake()
    {

    }

    private void Start()
    {
        pI = this.gameObject.GetComponent<PlayerInput>();
        controller = this.gameObject.GetComponent<CharacterController>();
        pI.pD.currentMovement = transform.position;
    }


    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        HandleMovement();
        //Debug.Log(speed);
    }
    
    private void HandleMovement()
    {
        Vector3 rawMovement = new Vector3(pI.GetHorizontalMovement() *.75f, 0.0f, pI.GetVerticalMovement());
        currentMovement = Vector3.MoveTowards(currentMovement, rawMovement, pI.pD.inertiaVar * Time.deltaTime);
        Vector3 finalMovement = transform.TransformVector(currentMovement);
        controller.Move(finalMovement * pI.pD.moveSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }





}
