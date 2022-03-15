using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    //[SerializeField] PlayerMovement playerMovement;

    [SerializeField] public PlayerData pD;
    [SerializeField] public GameObject hud;

    [SerializeField] public static event EventHandler InteractEvent;//
    [SerializeField] public static event EventHandler InteractCanceledEvent;
    [SerializeField] public static event EventHandler RotateEvent;//
    [SerializeField] public static event EventHandler RotateCanceledEvent;//
    
    [SerializeField] public static event EventHandler MoveObjEvent;


    [SerializeField] public static event EventHandler ResetObjEvent;//
    
    [SerializeField] public static event EventHandler PourEvent;
    
    [SerializeField] private PlayerControls pC;
    [SerializeField] private PlayerControls.FPPlayerActions fPP;
    [SerializeField] private InputAction interact;

    Vector2 mouseInput;
    Vector2 currentRotate;

    float horizontalMovement;
    float verticalMovement;
    Vector2 currentObjDistance;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pC = new PlayerControls();
        fPP = pC.FPPlayer;
        interact = pC.FPPlayer.Interact;

    }

    private void Start()
    {
        Instantiate(hud);
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
        interact.canceled += InteractCanceled;
        interact.Enable();


        fPP.Rotate.performed += ctx => currentRotate = ctx.ReadValue<Vector2>();
        fPP.Rotate.performed += Rotate;
        fPP.Rotate.canceled += RotateCanceled;
        fPP.Rotate.Enable();
        
        fPP.MoveObj.performed += ctx => currentObjDistance = ctx.ReadValue<Vector2>();
        fPP.MoveObj.performed += MoveObj;
        fPP.MoveObj.Enable();
    }

    private void MoveObj(InputAction.CallbackContext obj)
    {
        MoveObjEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        fPP.MoveForwardBackwards.Disable();
        fPP.MoveLeftRight.Disable();
        fPP.MouseX.Disable();
        fPP.MouseY.Disable();
        fPP.Jump.Disable();
        interact.Disable();
        fPP.Rotate.Disable();
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
    
    public float GetCurrentObjDistance()
    {
        
        return Mathf.Clamp(currentObjDistance.y, -1, 1);
    }

    public float GetMouseX()
    {
        return mouseInput.x;
    }

    public float GetMouseY()
    {
        return mouseInput.y;
    }

    public Vector2 GetCurrentRotate()
    {
        return currentRotate;
    }

    public void DoJump(InputAction.CallbackContext obj)
    {

    }

    public void Interact(InputAction.CallbackContext obj)
    {
        InteractEvent?.Invoke(this, EventArgs.Empty);
    }
    private void InteractCanceled(InputAction.CallbackContext obj)
    {
        InteractCanceledEvent?.Invoke(this, EventArgs.Empty);
    }
    private void Rotate(InputAction.CallbackContext obj)
    {
        RotateEvent?.Invoke(this, EventArgs.Empty);
    }

    private void RotateCanceled(InputAction.CallbackContext obj)
    {
        RotateCanceledEvent?.Invoke(this, EventArgs.Empty);
    }
}
