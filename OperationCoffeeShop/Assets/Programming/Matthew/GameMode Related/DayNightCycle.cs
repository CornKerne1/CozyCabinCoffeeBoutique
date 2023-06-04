using System;
using System.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    private float _lightIntensity;

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
        HourChanged += DayTimeCheck;
        HourChanged += NightTimeCheck;
    }

    public void HandleDnc()
    {
        StartTimer();
        SleepTimer();
        RotateSun();
    }
    public void StartTimer()
    {
        //Debug.Log(gMD.currentTime.ToString("HH:mm"));
        if (_gameModeData.currentTime.Hour >= 6 &&
            _gameModeData.currentTime.TimeOfDay.Hours <= _gameModeData.closingHour)
        {
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

    private void TrackTime()
    {
        if (_gameModeData.currentTime.Hour != _currentHour)
            HourChanged?.Invoke(this, EventArgs.Empty);
        _currentHour = _gameModeData.currentTime.Hour;
        _gameModeData.currentTime = _gameModeData.currentTime.AddSeconds(Time.deltaTime * _gameModeData.timeRate);
        TimeChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RotateSun()
    {
        float sunLightRotation;
        float moonLightRotation;

        if (_gameModeData.currentTime.TimeOfDay > _sunriseTime && _gameModeData.currentTime.TimeOfDay < _sunsetTime)
        {
            var sunriseToSunsetDuration = CalculateTimeDifference(_sunriseTime, _sunsetTime);
            var timeSinceSunrise = CalculateTimeDifference(_sunriseTime, _gameModeData.currentTime.TimeOfDay);

            var percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
            moonLightRotation = Mathf.Lerp(180, 360, -(float)percentage);
        }
        else
        {
            var sunsetToSunriseDuration = CalculateTimeDifference(_sunsetTime, _sunriseTime);
            var timeSinceSunset = CalculateTimeDifference(_sunsetTime, _gameModeData.currentTime.TimeOfDay);

            var percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
            moonLightRotation = Mathf.Lerp(180, 360, -(float)percentage);
        }
        if(!_gameMode) return;
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
    public async void NightTimeCheck(object sender, EventArgs e)
    {
        if (_gameModeData.currentTime.Hour != 20) return;
        var sunLightRef = _gameMode.sunLight.GetComponent<Light>();
        _lightIntensity = _gameMode.sunLight.GetComponent<Light>().intensity;
        float currentTime=0;
        while (currentTime < 2)
        {
            currentTime += Time.deltaTime;
            sunLightRef.intensity = Mathf.Lerp(sunLightRef.intensity, 0, currentTime);
            await Task.Yield();
        }
        sunLightRef.intensity = 0;
        RenderSettings.sun = sunLightRef;
        
    }

    public async void DayTimeCheck(object sender, EventArgs e)
    {
        if (_gameModeData.currentTime.Hour != 5) return;
        var sunLightRef = _gameMode.sunLight.GetComponent<Light>();
        float currentTime=0;
        while (currentTime < 2)
        {
            currentTime += Time.deltaTime;
            sunLightRef.intensity = Mathf.Lerp(sunLightRef.intensity, _lightIntensity, currentTime);//
            await Task.Yield();
        }
        sunLightRef.intensity = _lightIntensity;
    }
}