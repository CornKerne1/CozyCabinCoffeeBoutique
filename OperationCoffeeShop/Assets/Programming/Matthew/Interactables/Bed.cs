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
    private Collider playerCollider { get; set; }
    private PlayerCameraController playerCc { get; set; }
    private HeadBobController playerHbc { get; set; }
    private readonly float timeScaleFactor = 30;

    public void Update()
    {
        HandlePlayerMove();
    }

    public override void Start()
    {
        base.Start();
        var player = gameMode.player;
        playerCollider = player.GetComponent<Collider>();
        playerHbc = player.GetComponentInChildren<HeadBobController>();
        playerCc = player.GetComponent<PlayerCameraController>();
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

        MoveToAndRotateTowards(gameMode.playerData.sleeping ? sleepTrans.position : startTrans.position);
    }

    private void MoveToAndRotateTowards(Vector3 targetPos)
    {
        Quaternion lookRotation =
            Quaternion.LookRotation((targetPos + Vector3.up * 1.1f) - gameMode.camera.transform.position);
        gameMode.camera.transform.rotation =
            Quaternion.Lerp(gameMode.camera.transform.rotation, lookRotation, Time.deltaTime);
        _playerTrans.position = Vector3.Lerp(_playerTrans.position, targetPos, 0.5f * Time.deltaTime * .75f);
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
            playerCollider.enabled = true;
            _inBed = false;
            _playerInteraction = null;
            NewDay?.Invoke(this, EventArgs.Empty);
            playerHbc.enabled = true;
            playerCc.enabled = true;
            gameMode.gameModeData.timeRate = gameMode.gameModeData.timeRate/timeScaleFactor;
        }

        _coTimerRef = null;
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        if (gameMode.gameModeData.currentTime.Hour != 0 &&
            gameMode.gameModeData.currentTime.Hour < gameMode.gameModeData.closingHour) return;
        gameMode.Save(0);
        _playerInteraction = interaction;
        _playerTrans = gameMode.player.transform;
        gameMode.gameModeData.timeRate = timeScaleFactor * gameMode.gameModeData.timeRate;
        playerCollider.enabled = false;
        interaction.playerData.canMove = false;
        if (gameMode.gameModeData.currentTime.Hour != 0)
            gameMode.gameModeData.sleepDay = gameMode.gameModeData.currentTime.Day + 1;
        else
            gameMode.gameModeData.sleepDay = gameMode.gameModeData.currentTime.Day;
        gameMode.playerData.sleeping = true;
        _running = true;
        playerHbc.enabled = false;
        playerCc.enabled = false;
    }
}