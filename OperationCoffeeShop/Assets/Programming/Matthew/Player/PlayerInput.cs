using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] public PlayerData pD;
    [SerializeField] public GameObject hud;
    [SerializeField] private GameObject pauseM;
    [SerializeField] public static event EventHandler InteractEvent;
    [SerializeField] public static event EventHandler InteractCanceledEvent;
    [SerializeField] public static event EventHandler Alt_InteractEvent;

    [SerializeField] public static event EventHandler PauseEvent;
    [SerializeField] public static event EventHandler RotateEvent;
    [SerializeField] public static event EventHandler RotateCanceledEvent;
    [SerializeField] public static event EventHandler MoveObjEvent;
    [SerializeField] public static event EventHandler ResetObjEvent;

    [SerializeField] public static event EventHandler PourEvent;

    [SerializeField] private PlayerControls pC;
    [SerializeField] private PlayerControls.FPPlayerActions fPP;
    [SerializeField] private InputAction interact;
    [SerializeField] private InputAction alt_interact;
    [SerializeField] private InputAction pause;

    private Vector2 mouseInput;
    private Vector2 currentRotate;

    private float horizontalMovement;
    private float verticalMovement;
    private Vector2 currentObjDistance;

    public bool disabled;

    private void Update()
    {

    }

    private void Awake()
    {
        pC = new PlayerControls();
        fPP = pC.FPPlayer;
        interact = fPP.Interact;
        alt_interact = fPP.Alt_Interact;
        pause = fPP.PauseGame;

    }

    private void Start()
    {
        Instantiate(hud);
        pauseM.SetActive(false);
        pauseM.GetComponent<PauseMenu>().CloseOptions();
    }

    void _Pause()
    {
        if (!pD.inUI)
        {
            pauseM.SetActive(true);
            pauseM.GetComponent<PauseMenu>().pD = pD;
            pD.inUI = true;
        }
        else
        {
            if (pauseM)
            {
                pauseM.GetComponent<PauseMenu>().StartGame();
                pD.inUI = false;
            }
        }
    }

    public void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        disabled = false;

        fPP.Enable();
        fPP.MoveForwardBackwards.started += ctx => verticalMovement = ctx.ReadValue<float>();
        fPP.MoveForwardBackwards.canceled += ctx => verticalMovement = ctx.ReadValue<float>();
        fPP.MoveLeftRight.started += ctx => horizontalMovement = ctx.ReadValue<float>();
        fPP.MoveLeftRight.canceled += ctx => horizontalMovement = ctx.ReadValue<float>();

        fPP.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        fPP.MouseX.Enable();

        fPP.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        fPP.MouseY.Enable();

        interact.performed += Interact;
        interact.canceled += InteractCanceled;
        interact.Enable();

        alt_interact.performed += Alt_Interact;
        alt_interact.Enable();

        pause.canceled += Pause;
        pause.Enable();

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

    public void OnDisable()
    {
        disabled = true;
        fPP.MoveForwardBackwards.Disable();
        fPP.MoveLeftRight.Disable();
        fPP.MouseX.Disable();
        fPP.MouseY.Disable();
        interact.Disable();
        fPP.Rotate.Disable();
    }

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
    public void Interact(InputAction.CallbackContext obj)
    {
        InteractEvent?.Invoke(this, EventArgs.Empty);
    }
    private void InteractCanceled(InputAction.CallbackContext obj)
    {
        InteractCanceledEvent?.Invoke(this, EventArgs.Empty);
    }
    public void Alt_Interact(InputAction.CallbackContext obj)
    {
        Alt_InteractEvent?.Invoke(this, EventArgs.Empty);
    }
    public void Pause(InputAction.CallbackContext obj)
    {
        PauseEvent?.Invoke(this, EventArgs.Empty);
        _Pause();
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
