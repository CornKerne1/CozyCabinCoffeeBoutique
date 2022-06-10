using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]private Text fpsCounter;
    private void FixedUpdate()
    {
        var fps = (int)(1f / Time.unscaledDeltaTime);
        fpsCounter.text = fps + "FRAMES";
    }
}
