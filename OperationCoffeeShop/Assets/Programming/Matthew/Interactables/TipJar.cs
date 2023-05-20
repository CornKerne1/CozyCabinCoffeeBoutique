using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipJar : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction pI;
    [SerializeField] private GameObject halfJar;
    [SerializeField] private GameObject fullJar;

    public override void OnInteract(PlayerInteraction interaction)
    {
        this.pI = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        
    }
    
}
