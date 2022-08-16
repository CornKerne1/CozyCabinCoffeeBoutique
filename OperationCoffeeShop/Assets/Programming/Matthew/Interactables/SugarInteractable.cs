using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarInteractable : PhysicalIngredient
{
    // Start is called before the first frame update
    private PlayerInteraction pI;

    public override void OnInteract(PlayerInteraction interaction)
    {
        this.pI = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        
    }
    
}
