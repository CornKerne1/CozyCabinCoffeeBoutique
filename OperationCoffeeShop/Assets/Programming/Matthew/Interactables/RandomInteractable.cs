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
        if (outline)
        {
            outline.OutlineColor = outlineColor ;

        }
    }

    public override void OnLoseFocus()
    {
        if (outline)
        {
            var color = outlineColor;
            color.a = 0;
            outline.OutlineColor = color ;
        }
    }

    public override void OnAltInteract(PlayerInteraction pI)
    {
        
    }
    
}
