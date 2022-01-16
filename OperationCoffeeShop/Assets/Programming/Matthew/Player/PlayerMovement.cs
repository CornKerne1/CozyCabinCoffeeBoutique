using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    
    Vector3 verticalVelocity = Vector3.zero;
    
    private PlayerInput pI;
    private CharacterController controller;
    Vector3 currentVelocity;
    float gravity = -9.81f;
    float horizontalMovement;
    float verticalMovement;
    bool isGrounded;

    private void Start()
    {
        pI = this.gameObject.GetComponent<PlayerInput>();
        controller = this.gameObject.GetComponent<CharacterController>();
    }


    void Update()
    {
        HandleMovement();      
    }
    
    private void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, pI.pD.groundMask);
        if(isGrounded)
        {
            verticalVelocity.y = 0;
        }
        
        horizontalMovement = pI.GetHorizontalMovement();
        verticalMovement = pI.GetVerticalMovement();

        Vector3 movementVelocity = (transform.right * horizontalMovement + transform.forward * verticalMovement);
        controller.Move(movementVelocity * pI.pD.moveSpeed * Time.deltaTime);
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }





}
