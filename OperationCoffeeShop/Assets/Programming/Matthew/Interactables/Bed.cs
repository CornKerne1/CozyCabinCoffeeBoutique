using System;
using System.Collections;
using UnityEngine;

public class Bed : Interactable
{
    private PlayerInteraction _playerInteraction;

    [SerializeField] private Transform sleepTrans;
    [SerializeField] private Transform startTrans;
    private Transform _playerTrans;

    public float transportTime = 3f;

    public static event EventHandler NewDay; 


    private bool _running;

    private IEnumerator _coTimerRef;

    private bool _inBed;

    public void Update()
    {
        HandlePlayerMove();
    }

    private void HandlePlayerMove()
    {
        if (_playerInteraction)
        {
            if (!gameMode.gameModeData.sleeping && _playerInteraction.playerData.canMove == false)
            {
                _running = true;
                if (_coTimerRef == null)
                {
                    StartCoroutine(CO_Timer());
                }
            }
        }

        if (!_running) return;
        if (_coTimerRef == null)
        {
            StartCoroutine(CO_Timer());
        }

        if (gameMode.gameModeData.sleeping)
        {
            _playerTrans.position = Vector3.Lerp(_playerTrans.position, sleepTrans.position, 0.5f * Time.deltaTime);
        }
        else
        {
            _playerTrans.position = Vector3.Lerp(_playerTrans.position, startTrans.position, 0.5f * Time.deltaTime);
        }
    }

    private IEnumerator CO_Timer()
    {
        _coTimerRef = CO_Timer();
        yield return new WaitForSeconds(transportTime);
        if (!_inBed)
        {
            _running = false;
            _inBed = true;
        }
        else
        {
            _running = false;
            _playerInteraction.playerData.canMove = true;
            _playerTrans.GetComponent<Collider>().enabled = true;
            _inBed = false;
            gameMode.gameModeData.timeRate = gameMode.gameModeData.timeRate / 30;
            _playerInteraction = null;
            NewDay?.Invoke(this, EventArgs.Empty);
        }

        _coTimerRef = null;
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (gameMode.gameModeData.currentTime.Hour != 0 &&
            gameMode.gameModeData.currentTime.Hour < gameMode.gameModeData.closingHour) return;
        _playerInteraction = playerInteraction;
        _playerTrans = gameMode.player.transform;
        gameMode.gameModeData.timeRate = 30 * gameMode.gameModeData.timeRate;
        gameMode.player.GetComponent<Collider>().enabled = false;
        playerInteraction.playerData.canMove = false;
        if (gameMode.gameModeData.currentTime.Hour != 0)
            gameMode.gameModeData.sleepDay = gameMode.gameModeData.currentTime.Day + 1;
        else
            gameMode.gameModeData.sleepDay = gameMode.gameModeData.currentTime.Day;
        gameMode.gameModeData.sleeping = true;
        _running = true;
    }
}