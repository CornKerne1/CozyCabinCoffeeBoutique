using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[RequireComponent(typeof(DayCounter))]
public class Bed : Interactable
{
    private PlayerInteraction pI;

    [SerializeField] private Transform sleepTrans;
    [SerializeField] private Transform startTrans;
    private Transform _playerTrans;
    
    public float transportTime = 3f;

    private bool _running;

    private IEnumerator _coTimerRef;

    private bool _inBed;

    [FormerlySerializedAs("DayCounter")] public GameObject dayCounter;
    private GameObject _currentDc;
    private DayCounter _dC;
    public override void Start()
    {
        base.Start();
        _currentDc = Instantiate(dayCounter);
        _dC = _currentDc.GetComponent<DayCounter>();
        _dC.DisplayDay(gM.gMD.currentTime.Day);
        StartCoroutine(CO_RemoveDisplayDay());
    }

    private IEnumerator CO_RemoveDisplayDay()
    {
        yield return new WaitForSeconds(13f);
        StartCoroutine(_dC.CO_HideDisplay());
        _currentDc = null;
    }

    public void Update()
    {
        HandlePlayerMove();
    }

    private void HandlePlayerMove()
    {
        if (pI)
        {
            if (!base.gM.gMD.sleeping && pI.pD.canMove == false)
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
        if (base.gM.gMD.sleeping)
        {
            _playerTrans.position = Vector3.Lerp(_playerTrans.position, sleepTrans.position, 0.5f * Time.deltaTime);
        }
        else
        {
            if (!_currentDc)
            {
                _currentDc = Instantiate(dayCounter);
                _dC = _currentDc.GetComponent<DayCounter>();
                _dC.DisplayDay(gM.gMD.currentTime.Day);
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
            pI.pD.canMove = true;
            _playerTrans.GetComponent<Collider>().enabled = true;
            _inBed = false;
            base.gM.gMD.timeRate = base.gM.gMD.timeRate/30;
        }

        _coTimerRef = null;
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (gM.gMD.currentTime.Hour != 0 && gM.gMD.currentTime.Hour < gM.gMD.closingHour) return;
        _playerTrans = base.gM.player.transform;
        base.gM.gMD.timeRate = 30*base.gM.gMD.timeRate;
        base.gM.player.GetComponent<Collider>().enabled = false;
        playerInteraction.pD.canMove = false;
        if(gM.gMD.currentTime.Hour != 0)
            base.gM.gMD.sleepDay = gM.gMD.currentTime.Day + 1;
        else
            base.gM.gMD.sleepDay = gM.gMD.currentTime.Day;
        base.gM.gMD.sleeping = true;
        _running = true;
    }
}
