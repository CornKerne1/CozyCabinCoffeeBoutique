using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComputerShopInteractable : Interactable
{

    GameObject shopUI;

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
        Instantiate(shopUI);
        pcc.canMove = false;
        pm.canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void CloseShop()
    {
        shopUI.SetActive(false);
        pcc.canMove = true;
        pm.canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    public override void OnFocus()
    {
    }
    public override void OnLoseFocus()
    {
    }
}
