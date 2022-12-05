using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoScreen : MonoBehaviour
{
    private GameMode _gameMode;
    [SerializeField] private GameObject timeObj, dateObj, moneyObj, shopStatusObj;
    private TextMeshProUGUI time, date, money, shopStatus;
    private IEnumerator coRoutine;
    // Start is called before the first frame update
    void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        time = timeObj.GetComponent<TextMeshProUGUI>();
        date = dateObj.GetComponent<TextMeshProUGUI>();
        money = moneyObj.GetComponent<TextMeshProUGUI>();
        shopStatus = shopStatusObj.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (coRoutine == null)
            StartCoroutine(CO_FakeUpdate());
    }

    IEnumerator CO_FakeUpdate()
    {
        coRoutine = CO_FakeUpdate();
        yield return new WaitForSeconds(0.5f);
        UpdateVariables();
        coRoutine = null;
    }

    private void UpdateVariables()
    {
        var cT = _gameMode.gameModeData.currentTime;
        time.text =cT.ToString("HH:mm");
        date.text = cT.Month + "/" + cT.Day + "/" + cT.Year;
        money.text = "$" + _gameMode.gameModeData.moneyInBank;
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
