using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction pI;

    public override void OnInteract(PlayerInteraction pI)
    {
        if (!gM.gMD.isOpen)
        {
            gM.OpenShop();
        }
        
    }

    public override void OnFocus()
    {

    }

    public override void OnLoseFocus()
    {
        
    }
}
