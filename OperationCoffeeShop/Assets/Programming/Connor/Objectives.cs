using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Objectives : MonoBehaviour
{
    public List<TextMeshProUGUI> objectives;

    private TextMeshProUGUI _currentText;

    private GameMode _gameMode;

    [FormerlySerializedAs("ObjectiveCount")]
    public int objectiveCount;

    private float _dist;

    private bool _closingObjective;
    private bool _openingObjective;


    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        foreach (var text in objectives)
        {
            text.alpha = 0;
        }

        if (_gameMode.gameModeData.currentTime.Day != 1) return;
        _currentText = objectives[0];
        _currentText.alpha = 255;
        objectiveCount = 0;
    }

    private void Update()
    {
        if (_gameMode.gameModeData.currentTime.Hour >= _gameMode.gameModeData.wakeUpHour && _gameMode.gameModeData.currentTime.Minute >= 1 &&
            !_openingObjective)
        {
            ChangeObjective(++objectiveCount);
            _openingObjective = true;
        }

        if (_gameMode.gameModeData.currentTime.Hour < _gameMode.gameModeData.closingHour || _closingObjective) return;
        ChangeObjective(++objectiveCount);
        _closingObjective = true;
    }


    private void ChangeObjective(int i)
    {
        _currentText.alpha = 0;
        _currentText = objectives[i];
        _currentText.alpha = 255;
    }
}