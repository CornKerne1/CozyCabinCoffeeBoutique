using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fireflies : MonoBehaviour
{
    GameMode gM;
    ParticleSystem fireflies;
    // Start is called before the first frame update
    void Start()
    {
        this.gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        DayNightCycle.TimeChanged += SummonFireFlys;
        fireflies = gameObject.GetComponent<ParticleSystem>();
    }

    private void SummonFireFlys(object sender, EventArgs e)
    {

        if (gM.gMD.currentTime.Hour < gM.gMD.wakeUpHour || gM.gMD.currentTime.Hour > gM.gMD.closingHour - 1)
        {
            if (!fireflies.isPlaying)
                fireflies.Play();
        }
        else
        {
            if (fireflies.isPlaying)
                fireflies.Stop();
        }
    }

}
