using System;
using System.Collections;
using Unity.Mathematics;
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
            if (!gameMode.playerData.sleeping && _playerInteraction.playerData.canMove == false)
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

        if (gameMode.playerData.sleeping)
        {
            Quaternion lookRotation = Quaternion.LookRotation((sleepTrans.position+Vector3.up*1.1f) - gameMode.camera.transform.position);
            gameMode.camera.transform.rotation =
                Quaternion.Lerp(gameMode.camera.transform.rotation, lookRotation, Time.deltaTime);
            _playerTrans.position = Vector3.Lerp(_playerTrans.position, sleepTrans.position, 0.5f * Time.deltaTime*.75f);
        }
        else
        {
            Quaternion lookRotation = Quaternion.LookRotation((startTrans.position+Vector3.up*1.1f) - gameMode.camera.transform.position);
            gameMode.camera.transform.rotation =
                Quaternion.Lerp(gameMode.camera.transform.rotation, lookRotation, Time.deltaTime);
            _playerTrans.position = Vector3.Lerp(_playerTrans.position, startTrans.position, 0.5f * Time.deltaTime*.75f);
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
            _playerInteraction = null;
            NewDay?.Invoke(this, EventArgs.Empty);
            gameMode.player.GetComponentInChildren<HeadBobController>().enabled = true;
            gameMode.player.GetComponent<PlayerCameraController>().enabled = true;
        }

        _coTimerRef = null;
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        if (gameMode.gameModeData.currentTime.Hour != 0 &&
            gameMode.gameModeData.currentTime.Hour < gameMode.gameModeData.closingHour) return;
        _playerInteraction = interaction;
        _playerTrans = gameMode.player.transform;
        gameMode.gameModeData.timeRate = 30 * gameMode.gameModeData.timeRate;
        gameMode.player.GetComponent<Collider>().enabled = false;
        interaction.playerData.canMove = false;
        if (gameMode.gameModeData.currentTime.Hour != 0)
            gameMode.gameModeData.sleepDay = gameMode.gameModeData.currentTime.Day + 1;
        else
            gameMode.gameModeData.sleepDay = gameMode.gameModeData.currentTime.Day;
        gameMode.playerData.sleeping = true;
        _running = true;
        interaction.GetComponent<PlayerCameraController>().enabled = false;
        interaction.GetComponentInChildren<HeadBobController>().enabled = false;
    }
}