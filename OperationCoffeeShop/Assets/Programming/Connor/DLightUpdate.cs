using UnityEngine;
using System;
using UnityEngine.Serialization;

public class DLightUpdate : MonoBehaviour
{
    [FormerlySerializedAs("light")] public Light _light;
    public int wakeUpHour = 6;
    public int sleepingHour = 18;
    private GameMode _gameMode;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        if (_gameMode.gMD.currentTime.Hour >= wakeUpHour)
        {
            _light.enabled = true;
        }

        DayNightCycle.TimeChanged += AdjustLight;
    }

    private void AdjustLight(object sender, EventArgs e)
    {
        if (wakeUpHour == _gameMode.gMD.currentTime.Hour)
        {
            _light.enabled = true;
        }
        else if (sleepingHour == _gameMode.gMD.currentTime.Hour)
        {
            _light.enabled = false;
        }
    }
}