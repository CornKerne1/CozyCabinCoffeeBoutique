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
    public float speed =1;
    bool isGrounded;
    bool isMoving = false;


    private void Awake()
    {

    }

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
        
        if (pI.GetHorizontalMovement() == 0 && pI.GetVerticalMovement() == 0)
        {
            if(isMoving == true)
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
            horizontalMovement = pI.GetHorizontalMovement();
            verticalMovement = pI.GetVerticalMovement();
        }
        
        Vector3 movementVelocity = transform.right * horizontalMovement + transform.forward * verticalMovement;
        controller.Move(movementVelocity * speed * Time.deltaTime);
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

    }





}
