using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComputerShopInteractable : Interactable
{

    public Canvas shopUI;
    ComputerShop cS;
    PlayerMovement pm;
    PlayerCameraController pcc;

    private void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        cS = shopUI.gameObject.GetComponent<ComputerShop>();
        
        pcc = gM.player.gameObject.GetComponent<PlayerCameraController>();
        pm = gM.player.gameObject.GetComponent<PlayerMovement>();
    }

    public override void OnInteract(PlayerInteraction pI)
    {

        cS.balance.text = cS.balanceString + cS.cBTM.moneyInBank;
        shopUI.enabled = true;
        pcc.canMove = false;
        gM.pD.canMove = false;
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
