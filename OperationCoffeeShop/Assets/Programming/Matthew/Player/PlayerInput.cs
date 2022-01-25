using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    //[SerializeField] PlayerMovement playerMovement;

    public PlayerData pD;

    public static event EventHandler<EventArgs> InputEvents;
    public static event EventHandler<EventArgs> InteractEvent;

    public PlayerControls pC;
    private PlayerControls.FPPlayerActions fPP;
    private InputAction interact;

    Vector2 mouseInput;

    float horizontalMovement;
    float verticalMovement;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        pC = new PlayerControls();
        fPP = pC.FPPlayer;
        interact = pC.FPPlayer.Interact;
    }

    private void OnEnable()
    {
        fPP.Enable();
        fPP.MoveForwardBackwards.started += ctx => verticalMovement = ctx.ReadValue<float>();
        fPP.MoveForwardBackwards.canceled += ctx => verticalMovement = ctx.ReadValue<float>();
        fPP.MoveLeftRight.started += ctx => horizontalMovement = ctx.ReadValue<float>();
        fPP.MoveLeftRight.canceled += ctx => horizontalMovement = ctx.ReadValue<float>();

        fPP.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        //fPP.MouseX.canceled += ctx => mouseInput.x = ctx.ReadValue<float>();
        fPP.MouseX.Enable();

        fPP.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        //fPP.MouseY.canceled += ctx => mouseInput.y = ctx.ReadValue<float>();
        fPP.MouseY.Enable();

        fPP.Jump.performed += DoJump;
        fPP.Jump.Enable();

        interact.started += Interact;
        interact.Enable();
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        InteractEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        fPP.MoveForwardBackwards.Disable();
        fPP.MoveLeftRight.Disable();
        fPP.MouseX.Disable();
        fPP.MouseY.Disable();
        fPP.Jump.Disable();
        interact.Disable();
    }

    //private void Update()
    //{
    //    playerMovement.RecieveGroundMovementInput(groundMovement);
    //}

    public float GetHorizontalMovement()
    {
        return horizontalMovement;
    }

    public float GetVerticalMovement()
    {
        return -verticalMovement;
    }

    public float GetMouseX()
    {
        return mouseInput.x;
    }

    public float GetMouseY()
    {
        return mouseInput.y;
    }

    public void DoJump(InputAction.CallbackContext obj)
    {
        InputEvents?.Invoke(this, EventArgs.Empty);
    }



    //Vector2 moveInput = context.ReadValue<Vector2>();

}
