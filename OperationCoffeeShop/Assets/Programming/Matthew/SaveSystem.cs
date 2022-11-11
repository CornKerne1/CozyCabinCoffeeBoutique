using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem
{
    private readonly GameMode _gameMode;
    private readonly GameModeData _gameModeData;
    private SaveSystem _saveSystem;
    public SaveSystem(SaveSystem saveSystem, GameMode gameMode, GameModeData gameModeData)
    {
        _saveSystem = saveSystem;
        _gameMode = gameMode;
        _gameModeData = gameModeData;
    }

    public void SaveVariables()
    {
        var playerTrans = JsonUtility.ToJson(_gameMode.player.transform.position);
        PlayerPrefs.SetString("PlayerPosition", playerTrans);
        var date = JsonUtility.ToJson(_gameModeData.currentTime);
        PlayerPrefs.SetString("Date",date);
    }

    public void LoadVariables()
    {
        if (PlayerPrefs.HasKey("PlayerPosition"))
        {
            var playerPos = PlayerPrefs.GetString("PlayerPosition");
            _gameMode.player.position = JsonUtility.FromJson<Vector3>(playerPos);
        }

        if (PlayerPrefs.HasKey("Date"))
        {
            var date = JsonUtility.FromJson<DateTime>(PlayerPrefs.GetString("Date"));
            var newDate = new DateTime(date.Year, date.Month, date.Day, 5, 30, 0);
            _gameModeData.currentTime = newDate;
        }
        else
        {
            _gameModeData.startTime = new DateTime(2027, 1, 1, 5, 30, 0);
            _gameModeData.currentTime = _gameModeData.startTime;
        }
    }
}
