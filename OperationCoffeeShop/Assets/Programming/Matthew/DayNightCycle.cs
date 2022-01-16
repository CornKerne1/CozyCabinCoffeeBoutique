using System;//To use the EventHandler you must INCLUDE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle
{
    //Events!
    public event EventHandler TimeChanged;
    public GameModeData gMD;
    private DayNightCycle dNC;

    //Constructor!
    public DayNightCycle(DayNightCycle dNC, GameModeData gMD)
    {
        this.dNC = dNC;
        this.gMD = gMD;
    }
    //Handles store open timer.
    public void StartTimer()
    {
        //If store is open.
        if (gMD.isOpen)
        {
            //Subtracts the amount that passes from the variable.
            gMD.currentOpenTime -= 1 * Time.deltaTime;
            //If current time is less than or equal to 0.
            if(gMD.currentOpenTime <= 0)
            {
                UpdateTimeOfDay(gMD.hoursOpen);
                //Store closes.
                gMD.isOpen = false;
                //Timer Resets.
                gMD.currentOpenTime = gMD.openTimer;
            }
        }
    }
    
    public void Sub_TimeChanged(object sender, EventArgs e)
    {

    }
    
    //This updates and handles time of day when days have 24 hours..
    public void UpdateTimeOfDay(int time)
    {
        //If the current time is 2 in the morning, we want to display 2 not 26.
        if (gMD.timeOfDay + time >= 24)
        {
            gMD.timeOfDay = (gMD.timeOfDay + time) - 24;
            gMD.day = gMD.day + 1;
        }
        else gMD.timeOfDay += time;
        if (gMD.timeOfDay > 12) gMD.displayTime = -1 * (gMD.timeOfDay - 12);
        //If TimeChanged Event is not null (isValid?) Invoke Event. 
        TimeChanged?.Invoke(this, EventArgs.Empty);
    }
}
