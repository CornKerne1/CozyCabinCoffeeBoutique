using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]private Text fpsCounter;
    private void Update()
    {
        var fps = Time.frameCount / Time.time;
        fpsCounter.text = (int)fps + "FRAMES";
    }
}
