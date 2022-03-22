using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public float fps;
    public Text fpsCounter;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateFPS());
    }



    IEnumerator UpdateFPS()
    {
    yield return new WaitForSeconds(.2f);
    fps = (int)(1f / Time.unscaledDeltaTime);
    fpsCounter.text = fps + "FRAMES";
    StartCoroutine(UpdateFPS());
    }
}
