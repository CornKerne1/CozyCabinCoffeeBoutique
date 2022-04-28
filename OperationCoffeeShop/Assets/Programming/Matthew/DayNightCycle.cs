using System;//To use the EventHandler you must INCLUDE
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle
{
    //Events!
    public static event EventHandler TimeChanged;
    private GameMode gM;
    public GameModeData gMD;
    private DayNightCycle dNC;

    [SerializeField] float sunriseHour = 6;
    [SerializeField] float sunsetHour = 18;

    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    //Constructor!
    public DayNightCycle(DayNightCycle dNC, GameMode gM, GameModeData gMD)
    {
        this.dNC = dNC;
        this.gM = gM;
        this.gMD = gMD;
    }
    


    public void Initialize()
    {
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    //Handles store open timer.
    public void StartTimer()
    {
        //Debug.Log(gMD.currentTime.ToString("HH:mm"));
        if (gMD.isOpen)
        {
            //Subtracts the amount that passes from the variable.
            TrackTime();

            bool pastClosing = gMD.currentTime.TimeOfDay.Hours >= gMD.closingHour;
            
            if (gMD.isOpen && pastClosing)
            {
                gM.CloseShop();
            }
        }
    }
    
    public void SleepTimer()
    {
        if (gMD.sleeping)
        {
            TrackTime();
            if (gMD.currentTime >= gMD.sleepTime)
            {
                gMD.sleeping = false;
            }
        }
    }

    
    
    public void UpdateTimeOfDay(int hourAdding)
    {
        gMD.currentTime = gMD.currentTime.AddHours(hourAdding);
        //If TimeChanged Event is not null (isValid?) Invoke Event. 
        if (gMD.currentTime.Hour > 12) gMD.displayTime = -1 * (gMD.currentTime.Hour - 12);
        TimeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void TrackTime() 
    {
        gMD.currentTime = gMD.currentTime.AddSeconds(Time.deltaTime * gMD.timeRate);
        if (gMD.currentTime.Hour > 12) gMD.displayTime = -1 * (gMD.currentTime.Hour - 12);
        TimeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RotateSun()
    {
        float sunLightRotation;

        if(gMD.currentTime.TimeOfDay > sunriseTime && gMD.currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, gMD.currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, gMD.currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }
        gM.sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;
        if(difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }

}
