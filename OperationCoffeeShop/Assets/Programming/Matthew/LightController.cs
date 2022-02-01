using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light l;

    private GameMode gM;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        DayNightCycle.TimeChanged += Sub_TimeChanged;
        //StartCoroutine(Initialize());
    }

    void Sub_TimeChanged(object sender, EventArgs e)
    {        
        l.intensity = gM.gMD.currentTime.Hour;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //IEnumerator Initialize()
    //{
    //    yield return new WaitForSeconds(.02f);
    //    gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    //    gM.dNC.TimeChanged += Sub_TimeChanged;
    //}
}
