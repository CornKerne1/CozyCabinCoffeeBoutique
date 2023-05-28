using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{


    private PlayerInput _playerInput;
    [SerializeField] public CharacterController characterController;

    private Vector3 _velocity;
    private const float Gravity = -13f;
    private const float GroundDistance = .4f;
    [SerializeField] public bool isGrounded;
    [SerializeField] public LayerMask groundMask;
    private Vector3 _currentMovement;
    private Camera _camera;
    private Vector3 _camModeReset;
    private float _sprintModifier = 1;
    private float _crouchTime = .25f;
    private Vector3 _crouchingCenter = new Vector3(0,.7f,0);
    private Vector3 _standingCenter = new Vector3(0,.5f,0);
    private bool isCrouching;
    private Task _taskRunning;

    private void OnDestroy()
    {
        PlayerInput.CamModeEvent -= ToggleCamMode;
        PlayerInput.SprintEvent -= Sprint;
        PlayerInput.JumpEvent -= Jump;
        PlayerInput.CrouchEvent -= Crouch;
        Destroy(characterController);
    }

    private async void Awake()
    {
        _camera = Camera.main;
        characterController = GetComponent<CharacterController>();
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        _playerInput.pD.currentMovement = transform.position;
        _playerInput.pD.isClimbing = false;
        PlayerInput.CamModeEvent += ToggleCamMode;
        PlayerInput.SprintEvent += Sprint;
        PlayerInput.JumpEvent += Jump;
        PlayerInput.CrouchEvent += Crouch;
        await EditorFix();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, GroundDistance, groundMask);
        if (isGrounded && _velocity.y < 0)
            _velocity.y = -2f;
        if (_playerInput.pD.canMove)
            HandleMovement();
        if (_playerInput.GetVerticalMovement() != 0f) return;
        _playerInput.pD.isSprinting = false;
        _sprintModifier = 1;
    }

    public void TeleportPlayer(Vector3 destination)
    {
        characterController.enabled = false;
        transform.position = destination;
        characterController.enabled = true;
    }
    private async Task EditorFix()
    {
        var playerInteractable = GetComponent<PlayerInteraction>();
        playerInteractable.enabled = false;
        await Task.Delay(100);
        playerInteractable.enabled = true;

    }

    private void HandleMovement()
    {
        if (_playerInput.pD.isClimbing)
        {
            HandleLadderMovement();
        }
        else if ( _playerInput.pD.camMode)
        {
            if (!_camera) return;
            characterController.Move(_camera.transform.forward * (1.5f*_playerInput.pD.moveSpeed * Time.deltaTime * _playerInput.GetVerticalMovement()*2.5f));
            characterController.Move(_camera.transform.right * (1.5f*_playerInput.pD.moveSpeed * Time.deltaTime * _playerInput.GetHorizontalMovement()*2.5f));
        }
        else
        {
            Vector3 rawMovement = new Vector3(_playerInput.GetHorizontalMovement() * .75f, 0.0f, _playerInput.GetVerticalMovement()*_sprintModifier);
            _currentMovement = Vector3.MoveTowards(_currentMovement, rawMovement, _playerInput.pD.inertiaVar * Time.deltaTime);
            Vector3 finalMovement = transform.TransformVector(_currentMovement);
            characterController.Move( _playerInput.pD.moveSpeed * Time.deltaTime* finalMovement );
            _velocity.y += Gravity * Time.deltaTime;
            characterController.Move(_velocity * Time.deltaTime);
        }
    }
    
    private void HandleLadderMovement()
    {
        Vector3 lM = new Vector3(Vector3.up.x * _playerInput.GetVerticalMovement(), Vector3.up.y * _playerInput.GetVerticalMovement(),
            Vector3.up.z * _playerInput.GetVerticalMovement());
        _currentMovement = Vector3.MoveTowards(_currentMovement, lM, _playerInput.pD.inertiaVar * Time.deltaTime);
        Vector3 finalMovement = transform.TransformVector(_currentMovement);
        characterController.Move( _playerInput.pD.moveSpeed * Time.deltaTime * finalMovement);
        if (isGrounded && _playerInput.GetVerticalMovement() < 0)
        {
            _playerInput.pD.isClimbing = false;
        }
    }
    
    private async void Sprint(object sender, EventArgs e)
    {
        if(!_playerInput.pD.canMove) return;
        if(!_playerInput.pD.canSprint) return;
        if(isCrouching)await CrouchStandAsync();
        if (Math.Abs(_sprintModifier - 1) < .01f) //if sprint modifier == 1
        {
            _sprintModifier = _playerInput.pD.sprintSpeed;
            _playerInput.pD.isSprinting = true;
        }
        else
        {
            _sprintModifier = 1;
            _playerInput.pD.isSprinting = false;
        }
    }
    private async void Jump(object sender, EventArgs e)
    {
       if(!_playerInput.pD.canMove) return;
       if(!_playerInput.pD.canJump) return;
       if(!characterController.isGrounded) return;
       if (isCrouching) await CrouchStandAsync();
       var sM = _sprintModifier;
       _sprintModifier = 0;
       _velocity.y = _playerInput.pD.jumpHeight;
       _sprintModifier = sM;
    }

    private async void Crouch(object sender, EventArgs e)
    {
        if(!_playerInput.pD.canMove) return;
        if(!_playerInput.pD.canCrouch) return;
        if(!characterController.isGrounded) return;
        if (isCrouching && Physics.Raycast(_camera.transform.position, Vector3.up, .5f)) return;
        if (_taskRunning == null)
        {
            _taskRunning = CrouchStandAsync();
            await _taskRunning;
        }
    }

    private async Task CrouchStandAsync()
    {
        float timeElapsed = 0;
        var targetHeight = isCrouching ? _playerInput.pD.standHeight : _playerInput.pD.crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? _standingCenter : _crouchingCenter;
        Vector3 currentCenter = characterController.center;
        if (isCrouching)
        {
            _velocity.y = _playerInput.pD.jumpHeight / 1.25f;
            _sprintModifier = 1;
        }
        else
        {
            _sprintModifier = .5f;
        }

        while (timeElapsed < _crouchTime)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _crouchTime);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _crouchTime);
            timeElapsed += Time.deltaTime;
            await Task.Yield();
        }
        characterController.height = targetHeight;
        characterController.center = targetCenter;
        isCrouching = !isCrouching;
        _taskRunning = null;
    }

    private void ToggleCamMode(object sender, EventArgs e)
    {
        _playerInput.pD.camMode = ! _playerInput.pD.camMode;
        if ( _playerInput.pD.camMode)
        {
            _camModeReset = transform.position;
            var colliders= transform.root.GetComponentsInChildren<Collider>();
           foreach (var c in colliders)
           {
               if (c.GetInstanceID() ==characterController.GetInstanceID())
               {
                   gameObject.layer = 8;
                   return;
               }

               c.enabled = false;
           }
        }
        else
        {
            characterController.enabled = false;
            transform.position = _camModeReset;
            characterController.enabled = true;
            var colliders= transform.root.GetComponentsInChildren<Collider>();
            foreach (var c in colliders)
            {
                if (c.GetInstanceID() ==characterController.GetInstanceID())
                {
                    gameObject.layer = 2;
                    return;
                }
                c.enabled = true;
            }
        }
    }
}
