using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction pI;

    public Animator ButtonAnimator;
    public override void OnInteract(PlayerInteraction pI)
    {
        ButtonAnimator.SetTrigger("Press");
        if (!gM.gMD.isOpen)
        {
            gM.OpenShop();
            AkSoundEngine.PostEvent("Play_buttonpress" , this.gameObject);
        }
        
    }

    public override void OnFocus()
    {

    }

    public override void OnLoseFocus()
    {
        
    }

    public override void OnAltInteract(PlayerInteraction pI)
    {
        
    }
    
}
