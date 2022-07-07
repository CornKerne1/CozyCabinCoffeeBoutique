using UnityEngine;
using System;
using System.Collections;

public class DLightUpdate : MonoBehaviour
{
    private Light _light;
    public int wakeUpHour = 6;
    public int sleepingHour = 18;
    private GameMode _gameMode;

    private void Start()
    {
        StartCoroutine(CO_SuperStart());
    }

    private IEnumerator CO_SuperStart()
    {
        yield return new WaitForSeconds(.3f);
        _light = GetComponent<Light>();
        Debug.Log("let there be light");
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        if (_gameMode.gameModeData.currentTime.Hour >= wakeUpHour)
        {
            _light.enabled = true;
        }

        DayNightCycle.TimeChanged += AdjustLight;
    }

    private void AdjustLight(object sender, EventArgs e)
    {
        if (!_light) return;

        if (wakeUpHour == _gameMode.gameModeData.currentTime.Hour)
        {
            _light.enabled = true;
        }
        else if (sleepingHour == _gameMode.gameModeData.currentTime.Hour)
        {
            _light.enabled = false;
        }
    }
}