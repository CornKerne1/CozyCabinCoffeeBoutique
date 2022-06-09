using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform cam = null;

    private float _mouseX, _mouseY;
    private float _xRotation = 0f;

    private PlayerInput _playerInput;

    public GameObject holder;
    
    private void Start()
    {
        cam = GetComponent<Camera>().transform;
        _playerInput = GetComponent<PlayerInput>() != null ? GetComponent<PlayerInput>() : transform.root.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        CalculateDirection();
        PerformMovement();
    }

    

    private void CalculateDirection()
    {
        _xRotation -= _playerInput.GetMouseY() *
                      (_playerInput.pD.mouseSensitivity * _playerInput.pD.mouseSensitivityY / 400);
        _xRotation = Mathf.Clamp(_xRotation, -_playerInput.pD.neckClamp, _playerInput.pD.neckClamp);
        Vector3 camRotation = transform.eulerAngles;
        camRotation.x = _xRotation;
        cam.eulerAngles = new Vector3(camRotation.x, camRotation.y, camRotation.z);
    }
    private void PerformMovement()
    {
        if (_playerInput.pD.canMove)
            transform.root.Rotate(Vector3.up,
                _playerInput.pD.mouseSensitivity * _playerInput.pD.mouseSensitivityX * _playerInput.GetMouseX() *
                Time.deltaTime);
    }

}
