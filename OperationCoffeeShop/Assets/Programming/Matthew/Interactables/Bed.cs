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

    public override void Start()
    {
        base.Start();
        pI = base.gM.player.GetComponent<PlayerInteraction>();
    }

    public void Update()
    {
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
                playerTrans.position = Vector3.Lerp(playerTrans.position, sleepTrans.position, 1.0f * Time.deltaTime);
            }
            else
            {
                playerTrans.position = Vector3.Lerp(playerTrans.position, startTrans.position, 1.0f * Time.deltaTime);
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
            base.gM.gMD.timeRate = base.gM.gMD.timeRate/3;
        }

        TimerRef = null;
    }

    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        playerTrans = base.gM.player.transform;
        base.gM.gMD.timeRate = 3*base.gM.gMD.timeRate;
        base.gM.gMD.sleepTime = base.gM.gMD.currentTime.AddHours(8);
        //base.gM.gMD.sleepTime.AddHours(8);
        base.gM.gMD.sleeping = true;
        base.gM.player.GetComponent<Collider>().enabled = false;
        pI.pD.killSwitchOff = false;
        running = true;
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }
}
