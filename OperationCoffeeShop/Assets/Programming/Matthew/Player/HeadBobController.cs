using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{

    [SerializeField] private bool enable = true;

    [SerializeField, Range(0, .1f)] private float amplitude = .015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform holder;

    private PlayerMovement pM;
    
    private float toggleSpeed = 0.5f;
    private Vector3 startPos;
    private CharacterController controller;



    void Awake()
    {
        pM = gameObject.GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
        startPos = cam.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMotion();
        ResetPosition();
        cam.LookAt(FocusTarget());
    }

    private void PlayMotion(Vector3 motion)
    {
        cam.localPosition += motion;
    }

    private void CheckMotion()
    {
        //float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        if (pM.speed == 0) return;
        //if (!controller.isGrounded) return;

        PlayMotion(Motion());
    }

    private Vector3 Motion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }
    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + holder.localPosition.y, transform.position.z);
        pos += holder.forward * 15.0f;
        return pos;
    }
    
    private void ResetPosition()
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);
    }    
}
