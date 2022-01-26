using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    //[SerializeField] PlayerMovement playerMovement;

    [SerializeField] public PlayerData pD;

    [SerializeField] public static event EventHandler InteractEvent;//

    [SerializeField] private PlayerControls pC;
    [SerializeField] private PlayerControls.FPPlayerActions fPP;
    [SerializeField] private InputAction interact;

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

        interact.performed += Interact;
        interact.Enable();
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

    }

    public void Interact(InputAction.CallbackContext obj)
    {
        InteractEvent?.Invoke(this, EventArgs.Empty);
    }


}
