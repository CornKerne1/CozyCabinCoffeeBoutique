using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class Bed : Interactable
{
    private PlayerInteraction pI;

    [SerializeField] private Transform sleepTrans;
    [SerializeField] private Transform startTrans;
    private Transform playerTrans;
    
    public float transportTime = 3f;

    private bool running;

    private IEnumerator TimerRef;

    private bool inBed;

    public GameObject DayCounter;
    GameObject currentDC;
    DayCounter dC;
    public override void Start()
    {
        base.Start();
        pI = base.gM.player.GetComponent<PlayerInteraction>();
        currentDC = Instantiate(DayCounter);
        dC = currentDC.GetComponent<DayCounter>();
        dC.DisplayDay(gM.gMD.currentTime.Day);
        StartCoroutine(CO_RemoveDisplayDay());
    }

    IEnumerator CO_RemoveDisplayDay()
    {
        yield return new WaitForSeconds(15);
        Destroy(currentDC);
        currentDC = null;
    }

    public void Update()
    {
        //Debug.Log(base.gM.gMD.currentTime.Hour);
        HandlePlayerMove();
    }

    private void HandlePlayerMove()
    {
        if (pI)
        {
            if (!base.gM.gMD.sleeping && pI.pD.killSwitchOff == false)
            {
                running = true;
                if (TimerRef == null)
                {
                    StartCoroutine(Timer());
                }
            }
        }
        if (running)
        {
            if (TimerRef == null)
            {
                StartCoroutine(Timer());
            }
            if (base.gM.gMD.sleeping)
            {
                playerTrans.position = Vector3.Lerp(playerTrans.position, sleepTrans.position, 0.5f * Time.deltaTime);
            }
            else
            {
                if (currentDC == null)
                {
                    currentDC = Instantiate(DayCounter);
                    dC = currentDC.GetComponent<DayCounter>();
                    dC.DisplayDay(gM.gMD.currentTime.Day);
                }
               
                playerTrans.position = Vector3.Lerp(playerTrans.position, startTrans.position, 0.5f * Time.deltaTime);
                StartCoroutine(CO_RemoveDisplayDay());
            }
        }
    }

    IEnumerator Timer()
    {
        TimerRef = Timer();
        yield return new WaitForSeconds(transportTime);
        if (!inBed)
        {
            running = false;
            inBed = true;
        }
        else
        {
            running = false;
            pI.pD.killSwitchOff = true;
            playerTrans.GetComponent<Collider>().enabled = true;
            inBed = false;
            base.gM.gMD.timeRate = base.gM.gMD.timeRate/30;
        }

        TimerRef = null;
    }

    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (gM.gMD.currentTime.Hour == 18)//if (gM.gMD.currentTime.Hour > 18 ||gM.gMD.currentTime.Hour ==0)
        {
           
            playerTrans = base.gM.player.transform;
            base.gM.gMD.timeRate = 30*base.gM.gMD.timeRate;
            base.gM.player.GetComponent<Collider>().enabled = false;
            pI.pD.killSwitchOff = false;
            base.gM.gMD.sleepDay = gM.gMD.currentTime.Day + 1;
            base.gM.gMD.sleeping = true;
            running = true;
        }
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }


}
