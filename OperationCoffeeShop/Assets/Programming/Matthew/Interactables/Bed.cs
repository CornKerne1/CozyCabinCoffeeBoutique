using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    private PlayerMovement pM;
    private PlayerCameraController pCC;

    private GameObject cam;

    private bool running;
    public void Update()
    {
        
        if (!base.gM.gMD.sleeping && pM.enabled == false)
        {
            pM.enabled = true;
            pCC.enabled = true;
        }

        if (running)
        {
            if (!base.gM.gMD.sleeping)
            {
                //cam.transform.position
                //Quaternion.Lerp(door.transform.rotation, startTrans.rotation, 2.5f * Time.deltaTime);
            }
            else
            {
                
            }
        }
    }

    public override void Start()
    {
        base.Start();
        
        pM = gM.player.GetComponent<PlayerMovement>();
        pCC = gM.player.GetComponent<PlayerCameraController>();
        cam = pM.transform.root.GetComponentInChildren<Camera>().gameObject;
    }

    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        pM.enabled = false;
        pCC.enabled = false;
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
