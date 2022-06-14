using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComputerShopInteractable : Interactable
{

    public Canvas shopUI;
    private ComputerShop _computerShop;
    private PlayerMovement _playerMovement;
    private PlayerCameraController _playerCameraController;

    public override void Start()
    {
        base.Start();
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _computerShop = shopUI.gameObject.GetComponent<ComputerShop>();
        
        _playerCameraController = gM.player.gameObject.GetComponent<PlayerCameraController>();
        _playerMovement = gM.player.gameObject.GetComponent<PlayerMovement>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {

        _computerShop.balance.text = _computerShop.balanceString + _computerShop.coffeeBankTM.moneyInBank;
        shopUI.enabled = true;
        gM.pD.canMove = false;
        gM.pD.canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
