using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] public PlayerData pD;
    [SerializeField] public GameObject hud;
    [SerializeField] public GameObject hudRef;
    [SerializeField] private GameObject pauseM;
    [SerializeField]private GamepadCursor virtualCursor;
    public static event EventHandler SprintEvent;
    public static event EventHandler JumpEvent;
    public static event EventHandler CrouchEvent;
    public static event EventHandler InteractEvent;
    public static event EventHandler AltInteractEvent;
    public static event EventHandler RotateEvent;
    public static event EventHandler RotateCanceledEvent;
    public static event EventHandler MoveObjEvent;
    
    public static event EventHandler CamModeEvent;
    public static event EventHandler PauseEvent;

    private PlayerControls _pC;
    private PlayerControls.FPPlayerActions _fPp;
    private InputAction _interact;
    private InputAction _altInteract;
    private InputAction _pause;
    private InputAction _sprint;
    private InputAction _jump;
    private InputAction _crouch;

    private Vector2 _mouseInput;
    private Vector2 _currentRotate;
    private Vector2 _movement;
    
    private Vector2 _currentObjDistance;
    
    public String inputType;

    public bool disabled;
    private bool _movingObj,_rotatingObj;

    private async void Awake()
    {
        _pC = new PlayerControls();
        _fPp = _pC.FPPlayer;
        _interact = _fPp.Interact;
        _altInteract = _fPp.Alt_Interact;
        _pause = _fPp.PauseGame;
        _sprint = _fPp.Sprint;
        _jump = _fPp.Jump;
        _crouch = _fPp.Crouch;
    }
    private async void Start()
    {
        await InitializeHudAndCursor();
        await InitializeMenu();
        CamModeEvent += ToggleHud;
        PauseEvent += _Pause;
    }

    private Task InitializeHudAndCursor()
    {
        hudRef = Instantiate(hud);
        virtualCursor = hudRef.GetComponentInChildren<GamepadCursor>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        virtualCursor.playerInput = this;
        virtualCursor.transform.parent = null;
        return Task.CompletedTask;
    }

    private Task InitializeMenu()
    {
        virtualCursor.gameObject.SetActive(false);
        pauseM.SetActive(false);
        try
        {
            var pM = pauseM.GetComponent<PauseMenu>();
            pM.playerInput = this;
        }
        catch (Exception e)
        {
        }
        return Task.CompletedTask;
    }

    private void Update()
    {
        if (pD.inUI)
        {
            if(!virtualCursor.gameObject.activeSelf)
                virtualCursor.gameObject.SetActive(true);
        }
        else if (virtualCursor.gameObject.activeSelf)
            virtualCursor.gameObject.SetActive(false);
    }

    private void _Pause(object sender, EventArgs e)
    {
        //Debug.Log(pD.inUI);
        if (pD.inUI)
        {
            if (!pauseM.activeSelf) return;
            pauseM.GetComponent<PauseMenu>().StartGame();
            pD.inUI = false;
            ToggleHud();
        }
        else
        {
            pauseM.SetActive(true);
            pauseM.GetComponent<PauseMenu>().pD = pD;
            pD.inUI = true;
            ToggleHud(); 
        }
    }

    public async void OnEnable()
    {
        disabled = false;

        _fPp.Enable();
        
        _fPp.Mouse.started += OnMousePerformed;
        _fPp.Mouse.performed += OnMousePerformed;
        _fPp.Mouse.canceled += OnMousePerformed;
        _fPp.Mouse.Enable();

        _fPp.Move.started +=ctx=>_movement=ctx.ReadValue<Vector2>();
        _fPp.Move.performed +=ctx=>_movement=ctx.ReadValue<Vector2>();
        _fPp.Move.canceled +=ctx=>_movement=ctx.ReadValue<Vector2>();
        _fPp.Move.Enable();

        _sprint.performed += Sprint;
        _sprint.Enable();
        
        _jump.performed += Jump;
        _jump.Enable();
        
        _crouch.performed += Crouch;
        _crouch.Enable();
        
        _interact.performed += Interact;
        _interact.Enable();

        _altInteract.performed += Alt_Interact;
        _altInteract.Enable();

        _pause.canceled += Pause;
        _pause.Enable();

        _fPp.Rotate.performed += ctx => _currentRotate = ctx.ReadValue<Vector2>();
        _fPp.Rotate.performed += Rotate;
        _fPp.Rotate.canceled += RotateCanceled;
        _fPp.Rotate.Enable();
        _fPp.MoveObj.performed+= MoveObj;
        _fPp.MoveObj.Enable();
        
        //_fPp.FreeCam.performed += FreeCam;
        _fPp.FreeCam.Enable();
    }

    private async void OnMousePerformed(InputAction.CallbackContext ctx)
    {
        _mouseInput = ctx.ReadValue<Vector2>();
        inputType = ctx.control.ToString();
    }
    

    private async void MoveObj(InputAction.CallbackContext ctx)
    {
        if (_movingObj)
        {
            _movingObj = false;
            return;
        }
        _currentObjDistance = ctx.ReadValue<Vector2>();
        MoveObjEvent?.Invoke(this, EventArgs.Empty);
        inputType = ctx.control.ToString();
        if (inputType.Contains("dpad"))
        {
            _movingObj = true;
            await HandleDpadMovement();
        }
    }
    private async Task HandleDpadMovement()
    {
        while (_movingObj)
        {
            await Task.Delay(100);
            if (!_movingObj) return;
            MoveObjEvent?.Invoke(this, EventArgs.Empty);
        }
    }
    public async void OnDisable()
    {
        disabled = true;
        _fPp.Move.Disable();
        _fPp.Mouse.Disable();
        _sprint.Disable();
        _jump.Disable();
        _crouch.Disable();
        _interact.Disable();
        _altInteract.Disable();
        _fPp.Rotate.Disable();
        _fPp.FreeCam.Disable();
    }
    public void ToggleHud(object sender, EventArgs e)
    {
        hudRef.SetActive(!hudRef.activeSelf);
    }

    public float GetHorizontalMovement()
    {
        return _movement.x;
    }

    public float GetVerticalMovement()
    {
        return _movement.y;
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
    private async void Rotate(InputAction.CallbackContext obj)
    {
        RotateEvent?.Invoke(this, EventArgs.Empty);
    }

    private void RotateCanceled(InputAction.CallbackContext obj)
    {
        RotateCanceledEvent?.Invoke(this, EventArgs.Empty);
    }

    public void FreeCam()
    {
        CamModeEvent?.Invoke(null, EventArgs.Empty);
    }

    public void ToggleHud()
    {
        if(pD.inUI&& hudRef.activeSelf)
            hudRef.SetActive(false);
        else
            hudRef.SetActive(true);
            
    }

    public void ToggleMovement()
    {
        pD.canMove = !pD.canMove;
        pD.neckClamp = pD.neckClamp == 0 ? 77.3f : 0f;
    }

    private static void Pause(InputAction.CallbackContext obj)
    {
        PauseEvent?.Invoke(null, EventArgs.Empty);
    }

    private static void Sprint(InputAction.CallbackContext obj)
    {
        SprintEvent?.Invoke(null, EventArgs.Empty);
    }
    private static void Jump(InputAction.CallbackContext obj)
    {
        JumpEvent?.Invoke(null, EventArgs.Empty);
    }
    private static void Crouch(InputAction.CallbackContext obj)
    {
        CrouchEvent?.Invoke(null, EventArgs.Empty);
    }

    public InputAction GetMoveAction()
    {
        return _fPp.Move;
    }

    public PlayerInput GetPlayerInput()
    {
        return this;
    }
}