using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(DayCounter))]
public class Bed : Interactable
{
    private PlayerInteraction _playerInteraction;

    [SerializeField] private Transform sleepTrans;
    [SerializeField] private Transform startTrans;
    private Transform _playerTrans;

    public float transportTime = 3f;

    private bool _running;

    private IEnumerator _coTimerRef;

    private bool _inBed;

    [FormerlySerializedAs("DayCounter")] public GameObject dayCounter;
    [FormerlySerializedAs("_currentDc")] public GameObject currentDc;
    private DayCounter _dC;

    public override void Start()
    {
        base.Start();
        currentDc = Instantiate(dayCounter);
        _dC = currentDc.GetComponent<DayCounter>();
        _dC.DisplayDay(gameMode.gameModeData.currentTime.Day);
        StartCoroutine(CO_RemoveDisplayDay());
    }

    private IEnumerator CO_RemoveDisplayDay()
    {
        yield return new WaitForSeconds(13f);
        StartCoroutine(_dC.CO_HideDisplay());
        currentDc = null;
    }

    public void Update()
    {
        HandlePlayerMove();
    }

    private void HandlePlayerMove()
    {
        if (_playerInteraction)
        {
            if (!base.gameMode.gameModeData.sleeping && _playerInteraction.pD.canMove == false)
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

        if (base.gameMode.gameModeData.sleeping)
        {
            _playerTrans.position = Vector3.Lerp(_playerTrans.position, sleepTrans.position, 0.5f * Time.deltaTime);
        }
        else
        {
            if (!currentDc)
            {
                currentDc = Instantiate(dayCounter);
                _dC = currentDc.GetComponent<DayCounter>();
                _dC.DisplayDay(gameMode.gameModeData.currentTime.Day);
            }

            _playerTrans.position = Vector3.Lerp(_playerTrans.position, startTrans.position, 0.5f * Time.deltaTime);
            StartCoroutine(CO_RemoveDisplayDay());
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
            _playerInteraction.pD.canMove = true;
            _playerTrans.GetComponent<Collider>().enabled = true;
            _inBed = false;
            base.gameMode.gameModeData.timeRate = base.gameMode.gameModeData.timeRate / 30;
        }

        _coTimerRef = null;
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (gameMode.gameModeData.currentTime.Hour != 0 &&
            gameMode.gameModeData.currentTime.Hour < gameMode.gameModeData.closingHour) return;
        _playerTrans = base.gameMode.player.transform;
        gameMode.gameModeData.timeRate = 30 * base.gameMode.gameModeData.timeRate;
        gameMode.player.GetComponent<Collider>().enabled = false;
        playerInteraction.pD.canMove = false;
        if (gameMode.gameModeData.currentTime.Hour != 0)
            gameMode.gameModeData.sleepDay = gameMode.gameModeData.currentTime.Day + 1;
        else
            gameMode.gameModeData.sleepDay = gameMode.gameModeData.currentTime.Day;
        gameMode.gameModeData.sleeping = true;
        _running = true;
    }
}