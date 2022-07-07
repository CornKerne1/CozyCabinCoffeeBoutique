using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction pI;

    public Animator ButtonAnimator;
    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        ButtonAnimator.SetTrigger("Press");
        if (!gameMode.gameModeData.isOpen)
        {
            gameMode.OpenShop();
            AkSoundEngine.PostEvent("Play_buttonpress" , this.gameObject);
        }
        
    }
    public override void OnAltInteract(PlayerInteraction playerInteraction)
    {
        
    }
    
}
