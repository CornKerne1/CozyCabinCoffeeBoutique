using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RingTarget : MonoBehaviour
{
    public static event EventHandler RingToss;
    private bool _targetDown;
    private int _framesColliding;
    private bool _timerRunning;
    

    private void OnTriggerStay(Collider other)
    {
        _framesColliding++;
        if(!_timerRunning)
            StartCooldown();
        if (!_targetDown && _framesColliding>50)
        {
            _targetDown = true;
            RingToss?.Invoke(this.gameObject, EventArgs.Empty);
        }
    }

    private async void StartCooldown()
    {
        _timerRunning = true;
        var startTime = Time.time;
        var startFrames = _framesColliding;
        while (Time.time - startTime< 20)
        {
            if (startFrames < _framesColliding)
                break;
            await Task.Yield();
        }

        if (startFrames == _framesColliding)
            _framesColliding = 0;
        _timerRunning = false;
    }
}

