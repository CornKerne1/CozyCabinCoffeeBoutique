using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComputerShopInteractable : Interactable
{

    public Canvas shopUI;

    PlayerMovement pm;
    PlayerCameraController pcc;

    private void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        
        
        pcc = gM.player.gameObject.GetComponent<PlayerCameraController>();
        pm = gM.player.gameObject.GetComponent<PlayerMovement>();
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        shopUI.enabled = true;
        pcc.canMove = false;
        pm.canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    


    public override void OnFocus()
    {
    }
    public override void OnLoseFocus()
    {
    }
}
