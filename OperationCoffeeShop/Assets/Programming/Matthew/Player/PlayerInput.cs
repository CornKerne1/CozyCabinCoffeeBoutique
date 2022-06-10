using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] public PlayerData pD;
    [SerializeField] public GameObject hud;
    [SerializeField] private GameObject pauseM;
    [SerializeField] public static event EventHandler InteractEvent;
    [SerializeField] public static event EventHandler AltInteractEvent;
    [SerializeField] public static event EventHandler RotateEvent;
    [SerializeField] public static event EventHandler RotateCanceledEvent;
    [SerializeField] public static event EventHandler MoveObjEvent;
    
    [SerializeField] private PlayerControls _pC;
    [SerializeField] private PlayerControls.FPPlayerActions _fPp;
    [SerializeField] private InputAction interact;
    [SerializeField] private InputAction altInteract;
    [SerializeField] private InputAction pause;

    private Vector2 _mouseInput;
    private Vector2 _currentRotate;

    private float _horizontalMovement;
    private float _verticalMovement;
    private Vector2 _currentObjDistance;

    public bool disabled;
    private void Awake()
    {
        _pC = new PlayerControls();
        _fPp = _pC.FPPlayer;
        interact = _fPp.Interact;
        altInteract = _fPp.Alt_Interact;
        pause = _fPp.PauseGame;
    }
    private void Start()
    {
        Instantiate(hud);
        pauseM.SetActive(false);
        pauseM.GetComponent<PauseMenu>().CloseOptions();
    }

    private void _Pause()
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

        _fPp.Enable();
        _fPp.MoveForwardBackwards.started += ctx => _verticalMovement = ctx.ReadValue<float>();
        _fPp.MoveForwardBackwards.canceled += ctx => _verticalMovement = ctx.ReadValue<float>();
        _fPp.MoveLeftRight.started += ctx => _horizontalMovement = ctx.ReadValue<float>();
        _fPp.MoveLeftRight.canceled += ctx => _horizontalMovement = ctx.ReadValue<float>();

        _fPp.MouseX.performed += ctx => _mouseInput.x = ctx.ReadValue<float>();
        _fPp.MouseX.Enable();

        _fPp.MouseY.performed += ctx => _mouseInput.y = ctx.ReadValue<float>();
        _fPp.MouseY.Enable();

        interact.performed += Interact;
        interact.Enable();

        altInteract.performed += Alt_Interact;
        altInteract.Enable();

        pause.canceled += Pause;
        pause.Enable();

        _fPp.Rotate.performed += ctx => _currentRotate = ctx.ReadValue<Vector2>();
        _fPp.Rotate.performed += Rotate;
        _fPp.Rotate.canceled += RotateCanceled;
        _fPp.Rotate.Enable();

        _fPp.MoveObj.performed += ctx => _currentObjDistance = ctx.ReadValue<Vector2>();
        _fPp.MoveObj.performed += MoveObj;
        _fPp.MoveObj.Enable();
    }
    private void MoveObj(InputAction.CallbackContext obj)
    {
        MoveObjEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnDisable()
    {
        disabled = true;
        _fPp.MoveForwardBackwards.Disable();
        _fPp.MoveLeftRight.Disable();
        _fPp.MouseX.Disable();
        _fPp.MouseY.Disable();
        interact.Disable();
        altInteract.Disable();
        _fPp.Rotate.Disable();
    }
    public float GetHorizontalMovement()
    {
        return _horizontalMovement;
    }
    public float GetVerticalMovement()
    {
        return -_verticalMovement;
    }
    public float GetCurrentObjDistance()
    {

        return Mathf.Clamp(_currentObjDistance.y, -1, 1);
    }
    public float GetMouseX()
    {
        return _mouseInput.x;
    }
    public float GetMouseY()
    {
        return _mouseInput.y;
    }
    public Vector2 GetCurrentRotate()
    {
        return _currentRotate;
    }
    private void Interact(InputAction.CallbackContext obj)
    {
        InteractEvent?.Invoke(this, EventArgs.Empty);
    }
    private void Alt_Interact(InputAction.CallbackContext obj)
    {
        AltInteractEvent?.Invoke(this, EventArgs.Empty);
    }
    private void Pause(InputAction.CallbackContext obj)
    {
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
