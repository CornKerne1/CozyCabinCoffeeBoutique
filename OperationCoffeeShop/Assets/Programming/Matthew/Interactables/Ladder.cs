using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    public PlayerInteraction pI;
    public bool canClimb;
    
    public override void Start()
    {
        base.Start();
        canClimb = false;
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        this.pI = playerInteraction;
        if (!base.gM.gMD.isOpen)
        {
            if (this.pI.pD.isClimbing)
            {
                this.pI.pD.isClimbing = false;
            }
            else
            {
                if (canClimb)
                {
                    this.pI.pD.isClimbing = true;
                }
            }
        }
    }
}
