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

    public PlayerCameraController()
    {
    }
    
    private void Update()
    {
        HandleMovement();
    }

    public void HandleMovement()
    {

        transform.root.Rotate(Vector3.up, pI.pD.mouseSensitivityX * pI.GetMouseX() * Time.deltaTime);
        xRotation -= pI.GetMouseY() * (pI.pD.mouseSensitivityY / 400);
        xRotation = Mathf.Clamp(xRotation, -pI.pD.neckClamp, pI.pD.neckClamp);
        Vector3 camRotation = transform.eulerAngles;
        camRotation.x = xRotation;
        cam.eulerAngles = new Vector3(camRotation.x * Convert.ToInt16(pI.pD.killSwitchOff),camRotation.y * Convert.ToInt16(pI.pD.killSwitchOff), camRotation.z * Convert.ToInt16(pI.pD.killSwitchOff));
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
