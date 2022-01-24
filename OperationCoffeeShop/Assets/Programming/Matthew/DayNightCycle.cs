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
            TrackTime();
            //If current time is less than or equal to 0.
            if(gMD.currentTime.Hour >= gMD.closingHour)
            {
                gMD.isOpen = false;

            }
        }
    }
    
    public void Sub_TimeChanged(object sender, EventArgs e)
    {

    }
    
    //This updates and handles time of day when days have 24 hours..
    public void UpdateTimeOfDay(int hourAdding)
    {
        gMD.currentTime = gMD.currentTime.AddHours(hourAdding);
        //If TimeChanged Event is not null (isValid?) Invoke Event. 
        TimeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TrackTime() 
    {
        gMD.currentTime = gMD.currentTime.AddSeconds(Time.deltaTime * gMD.timeRate);
        TimeChanged?.Invoke(this, EventArgs.Empty);
    }
}
