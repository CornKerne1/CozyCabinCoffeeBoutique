using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCameraController : MonoBehaviour
{
    [FormerlySerializedAs("cam")] [SerializeField]
    private Transform _cam = null;

    private float _mouseX, _mouseY;
    private float _xRotation = 0f;

    private PlayerInput _playerInput;

    public GameObject holder;


    private void Start()
    {
        _cam = GetComponentInChildren<Camera>().transform;
        _playerInput = GetComponent<PlayerInput>() != null
            ? GetComponent<PlayerInput>()
            : transform.root.GetComponent<PlayerInput>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!_playerInput.pD.canMove) return;
        HandleFreeCamMovement();
        CalculateDirection();
        PerformMovement();
    }

    private void HandleFreeCamMovement()
    {

        _xRotation -= _playerInput.GetMouseY() *
                      (_playerInput.pD.mouseSensitivityOptions * _playerInput.pD.mouseSensitivityY / 400);
        _xRotation = Mathf.Clamp(_xRotation, -_playerInput.pD.neckClamp, _playerInput.pD.neckClamp);
        Vector3 camRotation = transform.eulerAngles;
        camRotation.x = _xRotation;
        _cam.eulerAngles = new Vector3(camRotation.x, camRotation.y, camRotation.z);
        transform.root.Rotate(Vector3.up,
            _playerInput.pD.mouseSensitivityOptions * _playerInput.pD.mouseSensitivityX * _playerInput.GetMouseX() *
            Time.deltaTime);
    }


    private void CalculateDirection()
    {
        _xRotation -= _playerInput.GetMouseY() *
                      (_playerInput.pD.mouseSensitivityOptions * _playerInput.pD.mouseSensitivityY / 400);
        _xRotation = Mathf.Clamp(_xRotation, -_playerInput.pD.neckClamp, _playerInput.pD.neckClamp);
        Vector3 camRotation = transform.eulerAngles;
        camRotation.x = _xRotation;
        _cam.eulerAngles = new Vector3(camRotation.x, camRotation.y, camRotation.z);
    }

    private void PerformMovement()
    {
        transform.root.Rotate(Vector3.up,
            _playerInput.pD.mouseSensitivityOptions * _playerInput.pD.mouseSensitivityX * _playerInput.GetMouseX() *
            Time.deltaTime);
    }
}
