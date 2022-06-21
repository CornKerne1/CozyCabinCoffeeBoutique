using UnityEngine;
using System;

public class Fireflies : MonoBehaviour
{
    private GameMode _gameMode;

    private ParticleSystem _fireflies;

    private void Start()
    {
        this._gameMode = GameObject.Find("GameMode").GetComponent<GameMode>();
        DayNightCycle.TimeChanged += SummonFireFlies;
        _fireflies = gameObject.GetComponent<ParticleSystem>();
    }

    private void SummonFireFlies(object sender, EventArgs e)
    {
        if (_gameMode.gameModeData.currentTime.Hour < _gameMode.gameModeData.wakeUpHour ||
            _gameMode.gameModeData.currentTime.Hour > _gameMode.gameModeData.closingHour - 1)
        {
            if (!_fireflies.isPlaying)
                _fireflies.Play();
        }
        else
        {
            if (_fireflies.isPlaying)
                _fireflies.Stop();
        }
    }
}