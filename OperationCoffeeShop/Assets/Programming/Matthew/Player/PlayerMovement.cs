using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    private PlayerInput _playerInput;
    [SerializeField] public CharacterController controller;

    private Vector3 _velocity;
    private const float Gravity = -9.81f;
    private const float GroundDistance = .4f;
    [SerializeField] public bool isGrounded;
    [SerializeField] public LayerMask groundMask;
    private Vector3 _currentMovement;
    private void Start()
    {
        _playerInput = this.gameObject.GetComponent<PlayerInput>();
        controller = this.gameObject.GetComponent<CharacterController>();
        _playerInput.pD.currentMovement = transform.position;
        _playerInput.pD.isClimbing = false;
        StartCoroutine(CO_EditorFix());
    }
    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, GroundDistance, groundMask);
        if (isGrounded && _velocity.y < 0)
            _velocity.y = -2f;
        if (_playerInput.pD.canMove)
            HandleMovement();
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
        else
        {
            Vector3 rawMovement = new Vector3(_playerInput.GetHorizontalMovement() * .75f, 0.0f, _playerInput.GetVerticalMovement());
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
}
