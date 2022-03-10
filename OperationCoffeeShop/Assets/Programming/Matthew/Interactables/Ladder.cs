using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Interactable
{
    public PlayerInteraction pI;
    private bool canClimb;
    
    void Start()
    {
        canClimb = false;
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
        if (canClimb)
        {
            this.pI.pD.isClimbing = true;
        }
    }

    public override void OnFocus()
    {
    }

    public override void OnLoseFocus()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(2))
        {
            canClimb = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(2))
        {
            canClimb = false;
            this.pI.pD.isClimbing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
