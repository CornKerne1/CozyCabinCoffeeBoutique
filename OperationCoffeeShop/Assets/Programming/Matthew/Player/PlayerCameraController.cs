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

    public GameObject holder;

    public bool canMove = true;

    public PlayerCameraController()
    {
    }

    private void Update()
    {
        //if (canMove)
        HandleMovement();
    }

    public void HandleMovement()
    {
        if (canMove)
            transform.root.Rotate(Vector3.up, pI.pD.mouseSensitivity * pI.pD.mouseSensitivityX * pI.GetMouseX() * Time.deltaTime);
        xRotation -= pI.GetMouseY() * (pI.pD.mouseSensitivity * pI.pD.mouseSensitivityY / 400);
        xRotation = Mathf.Clamp(xRotation, -pI.pD.neckClamp, pI.pD.neckClamp);
        Vector3 camRotation = transform.eulerAngles;
        camRotation.x = xRotation;
        cam.eulerAngles = new Vector3(camRotation.x, camRotation.y, camRotation.z);
    }

    private void Start()
    {
        cam = Camera.main.transform;
        if (this.gameObject.GetComponent<PlayerInput>() != null)
            pI = this.gameObject.GetComponent<PlayerInput>();
        else
            pI = transform.root.gameObject.GetComponent<PlayerInput>();

    }

}
