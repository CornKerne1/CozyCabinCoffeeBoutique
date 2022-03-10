using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInteractable : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction pI;

    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
        pI.Carry(gameObject);
    }

    public override void OnFocus()
    {

    }

    public override void OnLoseFocus()
    {
        
    }
}
