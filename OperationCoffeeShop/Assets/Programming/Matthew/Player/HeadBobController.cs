using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private PlayerData pD;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform holder;
    private Vector3 _startPos;
    private Vector3 _lastPosition;
    private CharacterController _controller;
    private float _sprintModifier = 1;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _startPos = cam.localPosition;
        _lastPosition = transform.root.position;
    }

    private void Update()
    {
        CheckMotion();
        ResetPosition();
        cam.LookAt(FocusTarget());
        _sprintModifier = pD.isSprinting?pD.sprintSpeed:1;
        _sprintModifier = pD.camMode ? 0:_sprintModifier;
    }

    private void CheckMotion()
    {
        if (GetSpeed() == 0) return;

        PlayMotion(Motion());
    }

    private void ResetPosition()
    {
        if (cam.localPosition == _startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, _startPos, 8 * Time.deltaTime);
    }

    private void PlayMotion(Vector3 motion)
    {
        cam.localPosition += motion * Time.deltaTime;
    }

    private Vector3 FocusTarget()
    {
        var currentPosition = transform.position;
        var pos = new Vector3(currentPosition.x, currentPosition.y + holder.localPosition.y, currentPosition.z);
        pos += holder.forward * 15.0f;
        return pos;
    }

    private float GetSpeed()
    {
        var currentRootTrans = transform.root.position;
        var speed = Vector3.Distance(_lastPosition, currentRootTrans);
        _lastPosition = currentRootTrans;
        return speed;
    }

    private Vector3 Motion()
    {
        if(!pD.isSprinting)
            _sprintModifier = 1;
        var pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * pD.frequency * _sprintModifier) * pD.amplitude * _sprintModifier;
        pos.x += Mathf.Cos(Time.time * pD.frequency * _sprintModifier / 2) * pD.amplitude * 2 * _sprintModifier;
        return pos;
    }
    
}
