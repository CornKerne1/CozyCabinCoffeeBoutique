using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DLightUpdate : MonoBehaviour
{
    public Light light;
    public int wakeUpHour = 6;
    public int sleepingHour = 18;
    GameMode gM;

    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

        DayNightCycle.TimeChanged += AdjustLight;
    }

    private void AdjustLight(object sender, EventArgs e)
    {
        Debug.Log(gM.gMD.currentTime.Hour);
        if (wakeUpHour == gM.gMD.currentTime.Hour)
        {
            light.enabled = true;
        }
        else if (sleepingHour == gM.gMD.currentTime.Hour)
        {
            light.enabled = false;
        }
    }
    

}
