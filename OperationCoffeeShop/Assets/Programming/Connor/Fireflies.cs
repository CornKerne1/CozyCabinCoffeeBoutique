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
        if (_gameMode.gMD.currentTime.Hour < _gameMode.gMD.wakeUpHour ||
            _gameMode.gMD.currentTime.Hour > _gameMode.gMD.closingHour - 1)
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