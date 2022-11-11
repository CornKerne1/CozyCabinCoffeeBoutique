using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{


    private PlayerInput _playerInput;
    [SerializeField] public CharacterController controller;

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
    private IEnumerator _coRunning;

    private void Start()
    {
        _camera = Camera.main;
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        controller = this.gameObject.GetComponent<CharacterController>();
        _playerInput.pD.currentMovement = transform.position;
        _playerInput.pD.isClimbing = false;
        PlayerInput.CamModeEvent += ToggleCamMode;
        PlayerInput.SprintEvent += Sprint;
        PlayerInput.JumpEvent += Jump;
        PlayerInput.CrouchEvent += Crouch;
        StartCoroutine(CO_EditorFix());
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

    private IEnumerator CO_EditorFix()
    {
        var playerInteractable = GetComponent<PlayerInteraction>();
        playerInteractable.enabled = false;
        yield return new WaitForSeconds(.1f);
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
            if (_camera)
            {
                controller.Move(_camera.transform.forward * (1.5f*_playerInput.pD.moveSpeed * Time.deltaTime * _playerInput.GetVerticalMovement()));
                controller.Move(_camera.transform.right * (1.5f*_playerInput.pD.moveSpeed * Time.deltaTime * _playerInput.GetHorizontalMovement()));
            }
        }
        else
        {
            Vector3 rawMovement = new Vector3(_playerInput.GetHorizontalMovement() * .75f, 0.0f, _playerInput.GetVerticalMovement()*_sprintModifier);
            _currentMovement = Vector3.MoveTowards(_currentMovement, rawMovement, _playerInput.pD.inertiaVar * Time.deltaTime);
            Vector3 finalMovement = transform.TransformVector(_currentMovement);
            controller.Move( _playerInput.pD.moveSpeed * Time.deltaTime* finalMovement );
            _velocity.y += Gravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
        }
    }
    
    private void HandleLadderMovement()
    {
        Vector3 lM = new Vector3(Vector3.up.x * _playerInput.GetVerticalMovement(), Vector3.up.y * _playerInput.GetVerticalMovement(),
            Vector3.up.z * _playerInput.GetVerticalMovement());
        _currentMovement = Vector3.MoveTowards(_currentMovement, lM, _playerInput.pD.inertiaVar * Time.deltaTime);
        Vector3 finalMovement = transform.TransformVector(_currentMovement);
        controller.Move( _playerInput.pD.moveSpeed * Time.deltaTime * finalMovement);
        if (isGrounded && _playerInput.GetVerticalMovement() < 0)
        {
            _playerInput.pD.isClimbing = false;
        }
    }
    
    private void Sprint(object sender, EventArgs e)
    {
        if(!_playerInput.pD.canMove) return;
        if(!_playerInput.pD.canSprint) return;
        if(isCrouching)StartCoroutine(CO_CrouchStand());
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
    private void Jump(object sender, EventArgs e)
    {
       if(!_playerInput.pD.canMove) return;
       if(!_playerInput.pD.canJump) return;
       if(!controller.isGrounded) return;
       if (isCrouching) StartCoroutine(CO_CrouchStand());
       var sM = _sprintModifier;
       _sprintModifier = 0;
       _velocity.y = _playerInput.pD.jumpHeight;
       _sprintModifier = sM;
    }

    private void Crouch(object sender, EventArgs e)
    {
        if(!_playerInput.pD.canMove) return;
        if(!_playerInput.pD.canCrouch) return;
        if(!controller.isGrounded) return;
        if (isCrouching && Physics.Raycast(_camera.transform.position, Vector3.up, .5f)) return;
        if(_coRunning==null)
            StartCoroutine(CO_CrouchStand());
    }

    private IEnumerator CO_CrouchStand()
    {
        _coRunning = CO_CrouchStand();
        float timeElapsed = 0;
        var targetHeight = isCrouching ? _playerInput.pD.standHeight : _playerInput.pD.crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? _standingCenter : _crouchingCenter;
        Vector3 currentCenter = controller.center;
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
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _crouchTime);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _crouchTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        controller.height = targetHeight;
        controller.center = targetCenter;
        isCrouching = !isCrouching;
        _coRunning = null;
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
               if (c.GetInstanceID() ==GetComponent<CharacterController>().GetInstanceID())
               {
                   gameObject.layer = 8;
                   return;
               }

               c.enabled = false;
           }
        }
        else
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = _camModeReset;
            GetComponent<CharacterController>().enabled = true;
            var colliders= transform.root.GetComponentsInChildren<Collider>();
            foreach (var c in colliders)
            {
                if (c.GetInstanceID() ==GetComponent<CharacterController>().GetInstanceID())
                {
                    gameObject.layer = 2;
                    return;
                }
                c.enabled = true;
            }
        }
    }
}
