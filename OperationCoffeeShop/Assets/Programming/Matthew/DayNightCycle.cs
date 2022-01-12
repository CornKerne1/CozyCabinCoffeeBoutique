using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle
{

    public event EventHandler TimeChanged;
    public GameModeData gMD;
    private DayNightCycle dNC;
    private GameMode gM;

    public DayNightCycle(DayNightCycle dNC, GameMode gameMode, GameModeData gameModeData)
    {
        this.dNC = dNC;
        this.gM = gameMode;
        this.gMD = gameModeData;
    }

    public void StartTimer()
    {
        if (gMD.isOpen)
        {
            gMD.currentOpenTime -= 1 * Time.deltaTime;
            if(gMD.currentOpenTime <= 0)
            {
                UpdateTimeOfDay(gMD.hoursOpen);
                gMD.isOpen = false;
                gMD.currentOpenTime = gMD.openTimer;
            }
        }
    }
    public void UpdateTimeOfDay(int time)
    {

        if (gMD.timeOfDay + time > 24) gMD.timeOfDay = (gMD.timeOfDay + time) - 24;
        else gMD.timeOfDay += time;
        if (gMD.timeOfDay > 12) gMD.displayTime = -1 * (gMD.timeOfDay - 12);

        TimeChanged?.Invoke(this, EventArgs.Empty);
    }
}
