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
    public bool canMove = true;

    private void Awake()
    {

    }

    private void Start()
    {
        pI = this.gameObject.GetComponent<PlayerInput>();
        controller = this.gameObject.GetComponent<CharacterController>();
        pI.pD.currentMovement = transform.position;
        pI.pD.isClimbing = false;
        StartCoroutine(EditorFix());
    }

    private IEnumerator EditorFix()
    {
        var pInt = GetComponent<PlayerInteraction>();
        pInt.enabled = false;
        yield return new WaitForSeconds(.1f);
        pInt.enabled = true;

    }


    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f * Convert.ToInt16(pI.pD.killSwitchOff);
        }
        if (canMove)
            HandleMovement();
        //Debug.Log(speed);
    }

    private void HandleMovement()
    {
        if (pI.pD.isClimbing)
        {
            HandleLadderMovement();
        }
        else
        {
            Vector3 rawMovement = new Vector3(pI.GetHorizontalMovement() * .75f, 0.0f, pI.GetVerticalMovement());
            currentMovement = Vector3.MoveTowards(currentMovement, rawMovement, pI.pD.inertiaVar * Time.deltaTime);
            Vector3 finalMovement = transform.TransformVector(currentMovement);
            controller.Move(finalMovement * pI.pD.moveSpeed * Time.deltaTime * Convert.ToInt16(pI.pD.killSwitchOff));
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime * Convert.ToInt16(pI.pD.killSwitchOff));
        }
    }
    private void HandleLadderMovement()
    {
        Vector3 lM = new Vector3(Vector3.up.x * pI.GetVerticalMovement(), Vector3.up.y * pI.GetVerticalMovement(),
            Vector3.up.z * pI.GetVerticalMovement());
        currentMovement = Vector3.MoveTowards(currentMovement, lM, pI.pD.inertiaVar * Time.deltaTime);
        Vector3 finalMovement = transform.TransformVector(currentMovement);
        controller.Move(finalMovement * pI.pD.moveSpeed * Time.deltaTime * Convert.ToInt16(pI.pD.killSwitchOff));
        if (isGrounded && pI.GetVerticalMovement() < 0)
        {
            pI.pD.isClimbing = false;
        }
    }





}
