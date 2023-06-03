using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class InfoScreen : MonoBehaviour
{
    private GameMode _gameMode;
    [SerializeField] private GameObject timeObj, dateObj, moneyObj, shopStatusObj,playerNamePlateObj;
    private TextMeshProUGUI time, date, money, shopStatus, namePlate;
    private Task _task;
    // Start is called before the first frame update
    private async void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        time = timeObj.GetComponent<TextMeshProUGUI>();
        date = dateObj.GetComponent<TextMeshProUGUI>();
        money = moneyObj.GetComponent<TextMeshProUGUI>();
        shopStatus = shopStatusObj.GetComponent<TextMeshProUGUI>();
        namePlate = playerNamePlateObj.GetComponent<TextMeshProUGUI>();
        if (_task == null)
        {
            _task = FakeUpdate();
            await _task;
        }
    }

    private async Task FakeUpdate()
    {
        UpdateVariables();
        await Task.Delay(500);
        _task = null;
        _task = FakeUpdate();
        await _task;
    }

    private void UpdateVariables()
    {
        var cT = _gameMode.gameModeData.currentTime;
        time.text =cT.ToString("HH:mm");
        date.text = cT.Month + "/" + cT.Day + "/" + cT.Year;
        money.text = "$" + _gameMode.gameModeData.moneyInBank;
        namePlate.text = _gameMode.playerData.playerName;
        if (_gameMode.gameModeData.isOpen)
        {
            shopStatus.text = "OPEN";
            shopStatus.color = Color.green;
        }
        else
        {
            shopStatus.text = "CLOSED";
            shopStatus.color = Color.red;
        }
    }
}
