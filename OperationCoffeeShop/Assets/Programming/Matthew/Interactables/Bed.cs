using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bed : Interactable
{
    private PlayerInteraction pI;

    [SerializeField] private Transform sleepTrans;
    [SerializeField] private Transform startTrans;

    private bool running;

    public override void Start()
    {
        base.Start();
        pI = base.gM.player.GetComponent<PlayerInteraction>();
    }

    public void Update()
    {
        if (pI)
        {
            if (!base.gM.gMD.sleeping && pI.pD.killSwitchOff == false)
            {
                running = true;
            }
        }

        if (running)
        {
            if (base.gM.gMD.sleeping)
            {
                gM.player.transform.position = Vector3.Lerp(gM.player.transform.position, sleepTrans.position, 1.0f * Time.deltaTime);
                gM.player.transform.rotation = Quaternion.Lerp(gM.player.transform.rotation, sleepTrans.rotation, 1.0f * Time.deltaTime);
                if (gM.player.transform.position == sleepTrans.position)
                {
                    running = false;
                }
            }
            else
            {
                Debug.Log("sometin");
                gM.player.transform.position = Vector3.Lerp(gM.player.transform.position, startTrans.position, 1.0f * Time.deltaTime);
                gM.player.transform.rotation = Quaternion.Lerp(gM.player.transform.rotation, startTrans.rotation, 1.0f * Time.deltaTime);
                if (gM.player.transform.position == startTrans.position)
                {
                    running = false;
                    pI.pD.killSwitchOff = true;

                }
            }
        }
    }

    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        startTrans = pI.gameObject.transform;
        pI.pD.killSwitchOff = false;
        base.gM.gMD.sleepTime = base.gM.gMD.currentTime.AddHours(8);
        //base.gM.gMD.sleepTime.AddHours(8);
        base.gM.gMD.sleeping = true;
        running = true;
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }
}
