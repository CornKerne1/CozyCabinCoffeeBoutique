using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraScript : MonoBehaviour
{
    public Transform car;
    public float distance = 6.4f;
    public float height = 1.4f;
    public float rotationDamping = 3.0f;
    public float heightDamping = 2.0f;
    public float zoomRatio = 0.5f;
    public float defaultFOV = 60f;

    private Vector3 rotationVector;

    private GameMode gameMode;
    private PlayerInput playerInput;

    private float currentAngle;
    private float currentHeight;

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        playerInput = gameMode.player.GetComponent<PlayerInput>();
    }

    void LateUpdate()
    {
        Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().velocity);
        if (localVelocity.z < -0.1f)
        {
            Vector3 temp = rotationVector;
            temp.y = car.eulerAngles.y + 180;
            rotationVector = temp;
        }
        else
        {
            Vector3 temp = rotationVector;
            temp.y = car.eulerAngles.y;
            rotationVector = temp;
        }

        // Smoothly interpolate the field of view
        float acc = car.GetComponent<Rigidbody>().velocity.magnitude;
        float targetFOV = defaultFOV + acc * zoomRatio;
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, targetFOV, Time.deltaTime * 2f);
    }

    void FixedUpdate()
    {
        // Calculate the target position based on the car's position and the camera's distance and height
        float wantedAngle = rotationVector.y;
        float wantedHeight = car.position.y + height;

        // Smoothly interpolate the camera's angle and height
        currentAngle = Mathf.LerpAngle(currentAngle, wantedAngle, Time.deltaTime * rotationDamping);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, Time.deltaTime * heightDamping);

        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);
        Vector3 targetPosition = car.position - currentRotation * Vector3.forward * distance;
        targetPosition.y = currentHeight;

        // Check for obstacles in the camera's path and adjust its position if necessary
        Vector3 start = car.position + Vector3.up * 0.5f; // start just above the car
        Vector3 end = targetPosition + Vector3.up * 0.5f; // end just above the target position
        float radius = .7f; // adjust this to the size of your camera

        RaycastHit hit;
        if (Physics.CapsuleCast(start, end, radius, Vector3.forward, out hit, distance))
        {
            targetPosition = hit.point + hit.normal * 0.2f; // add a small offset to prevent clipping

        }

        // Update the camera's position and rotation
        transform.position = targetPosition;
        transform.LookAt(car);
    }
}