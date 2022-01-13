using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle
{

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
        }
    }
    public void UpdateTimeOfDay(int time)
    {
        if (gMD.timeOfDay + time > 24)
        {
            gMD.timeOfDay = (gMD.timeOfDay + time) - 24;
        }
        gMD.timeOfDay += time;
    }
}
