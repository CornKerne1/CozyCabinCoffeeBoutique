using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInteractable : Interactable
{
    // Start is called before the first frame update


    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        interaction.Throw();
    }

}