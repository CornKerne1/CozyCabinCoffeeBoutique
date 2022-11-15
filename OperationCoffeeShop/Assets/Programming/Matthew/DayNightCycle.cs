using System;
using UnityEngine;

public class DayNightCycle
{
    public static event EventHandler TimeChanged;
    public static event EventHandler HourChanged;
    private readonly GameMode _gameMode;
    private readonly GameModeData _gameModeData;
    private DayNightCycle _dayNightCycle;

    [SerializeField] private const float SunriseHour = 6;
    [SerializeField] private const float SunsetHour = 18;

    private TimeSpan _sunriseTime;
    private TimeSpan _sunsetTime;
    private float _startTimeRate;
    private int _currentHour;

    public DayNightCycle(DayNightCycle dayNightCycle, GameMode gameMode, GameModeData gameModeData)
    {
        _dayNightCycle = dayNightCycle;
        _gameMode = gameMode;
        _gameModeData = gameModeData;
    }


    public void Initialize()
    {
        _sunriseTime = TimeSpan.FromHours(SunriseHour);
        _sunsetTime = TimeSpan.FromHours(SunsetHour);
        _startTimeRate = _gameModeData.timeRate;
    }

    public void StartTimer()
    {
        //Debug.Log(gMD.currentTime.ToString("HH:mm"));
        if (_gameModeData.currentTime.Hour >= 6 &&
            _gameModeData.currentTime.TimeOfDay.Hours <= _gameModeData.closingHour)
        {
            //if (!gMD.isOpen)
            //{
            //gM.OpenShop();
            //}
            TrackTime();

            var pastClosing = _gameModeData.currentTime.TimeOfDay.Hours >= _gameModeData.closingHour;

            if (_gameModeData.isOpen && pastClosing)
                _gameMode.CloseShop();
        }
        else if (_gameModeData.currentTime.Hour != 0 && !_gameMode.playerData.sleeping)
        {
            TrackTime();
        }
    }

    public void SleepTimer()
    {
        if (!_gameMode.playerData.sleeping) return;
        TrackTime();

        if (_gameModeData.currentTime.Hour != 6 || _gameModeData.currentTime.Day != _gameModeData.sleepDay) return;
        _gameMode.playerData.sleeping = false;
        _gameModeData.timeRate = _startTimeRate;
    }


    public void UpdateTimeOfDay(int hourAdding)
    {
        _gameModeData.currentTime = _gameModeData.currentTime.AddHours(hourAdding);
        //If TimeChanged Event is not null (isValid?) Invoke Event. 
        if (_gameModeData.currentTime.Hour > 12) _gameModeData.displayTime = -1 * (_gameModeData.currentTime.Hour - 12);
        TimeChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TrackTime()
    {
        if (_gameModeData.currentTime.Hour != _currentHour)
            HourChanged?.Invoke(this, EventArgs.Empty);
        _currentHour = _gameModeData.currentTime.Hour;
        _gameModeData.currentTime = _gameModeData.currentTime.AddSeconds(Time.deltaTime * _gameModeData.timeRate);
        if (_gameModeData.currentTime.Hour > 12) _gameModeData.displayTime = -1 * (_gameModeData.currentTime.Hour - 12);
        TimeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RotateSun()
    {
        float sunLightRotation;

        if (_gameModeData.currentTime.TimeOfDay > _sunriseTime && _gameModeData.currentTime.TimeOfDay < _sunsetTime)
        {
            var sunriseToSunsetDuration = CalculateTimeDifference(_sunriseTime, _sunsetTime);
            var timeSinceSunrise = CalculateTimeDifference(_sunriseTime, _gameModeData.currentTime.TimeOfDay);

            var percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            var sunsetToSunriseDuration = CalculateTimeDifference(_sunsetTime, _sunriseTime);
            var timeSinceSunset = CalculateTimeDifference(_sunsetTime, _gameModeData.currentTime.TimeOfDay);

            var percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        _gameMode.sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private static TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        var difference = toTime - fromTime;
        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}