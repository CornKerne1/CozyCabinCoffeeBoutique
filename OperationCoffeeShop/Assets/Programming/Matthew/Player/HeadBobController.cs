using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private PlayerData pD;

    [SerializeField] private bool enable = true;

    [SerializeField] private Transform cam;
    [SerializeField] private Transform holder;

    
    private Vector3 startPos;
    private Vector3 lastPosition;
    private CharacterController controller;



    void Awake()
    {
        controller = GetComponent<CharacterController>();
        startPos = cam.localPosition;
        lastPosition = transform.root.position;
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
        cam.localPosition += motion * Time.deltaTime;
    }

    private void CheckMotion()
    {
        if (GetSpeed() == 0) return;

        PlayMotion(Motion());
    }

    private float GetSpeed()
    {
        float speed = Vector3.Distance(lastPosition, transform.root.position);
        lastPosition = transform.root.position;
        return speed;
    }
    private Vector3 Motion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * pD.frequency) * pD.amplitude;
        pos.x += Mathf.Cos(Time.time * pD.frequency / 2) * pD.amplitude * 2;
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
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 8 * Time.deltaTime);
    }    
}
