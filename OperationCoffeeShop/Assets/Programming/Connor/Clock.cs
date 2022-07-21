using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform hoursPivot, minutesPivot, secondsPivot;

    private const float HoursToDegrees = 30f, MinutesToDegrees = 6f, SecondsToDegrees = 6f;

    private GameMode _gameMode;

    private TimeSpan _timeSpan;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    private void Update()
    {
        _timeSpan = _gameMode.gameModeData.currentTime.TimeOfDay;
        if (hoursPivot)
            hoursPivot.localRotation = Quaternion.Euler(-HoursToDegrees * (float)_timeSpan.TotalHours, 0f, 0f);
        if (minutesPivot)
            minutesPivot.localRotation = Quaternion.Euler(-MinutesToDegrees * (float)_timeSpan.TotalMinutes, 0f, 0f);
        if (secondsPivot)
            secondsPivot.localRotation = Quaternion.Euler(-SecondsToDegrees * (float)_timeSpan.TotalSeconds, 0f, 0f);
    }
}