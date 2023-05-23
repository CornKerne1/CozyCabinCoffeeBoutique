using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class BeachBall : Interactable
{
    // Start is called before the first frame update
    private Transform _playerCollision;

    public override void Awake()
    {
        base.Awake();
        _playerCollision = transform.GetChild(0);
        _playerCollision.gameObject.layer = 0;
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
        _playerCollision.gameObject.SetActive(false);
    }

    public override void OnDrop()
    {
        base.OnDrop();
        _playerCollision.gameObject.SetActive(true);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        interaction.Throw();
    }

}