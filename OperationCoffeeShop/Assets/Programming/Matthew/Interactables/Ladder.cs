using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    public PlayerInteraction pI;
    public bool canClimb;
    
    void Start()
    {
        canClimb = false;
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
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

    public override void OnFocus()
    {
    }

    public override void OnLoseFocus()//
    {
    }
}
