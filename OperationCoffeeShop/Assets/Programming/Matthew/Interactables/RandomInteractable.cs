using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInteractable : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction _pI;


    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        this._pI = playerInteraction;
        playerInteraction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction playerInteraction)
    {
    }
}