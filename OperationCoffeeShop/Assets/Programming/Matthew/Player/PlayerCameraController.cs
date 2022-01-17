using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] Transform cam = null;
    
    float mouseX, mouseY;
    float xRotation = 0f;

    private PlayerInput pI;

    private void Update()
    {
        HandleMovement();
    }

    public void HandleMovement()
    {
        transform.Rotate(Vector3.up, pI.pD.mouseSensitivityX * pI.GetMouseX() * Time.deltaTime);
        xRotation -= pI.GetMouseY() * (pI.pD.mouseSensitivityY / 200);
        xRotation = Mathf.Clamp(xRotation, -pI.pD.neckClamp, pI.pD.neckClamp);
        Vector3 camRotation = transform.eulerAngles;
        camRotation.x = xRotation;
        cam.eulerAngles = camRotation;
    }

    private void Start()
    {
        pI = this.gameObject.GetComponent<PlayerInput>();

    }

}
