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
            if (_fireflies.isPlaying) return;
            _fireflies.Play();
            AkSoundEngine.PostEvent("PLAY_FIREFLIES", gameObject);


        }
        else
        {
            if (!_fireflies.isPlaying) return;
            _fireflies.Stop();
            AkSoundEngine.PostEvent("STOP_FIREFLIES", gameObject);


        }
    }

    private void OnDestroy()
    {
        DayNightCycle.TimeChanged -= SummonFireFlies;
    }
}