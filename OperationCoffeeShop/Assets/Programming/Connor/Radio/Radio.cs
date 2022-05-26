using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : Interactable
{
    bool isOn;
    public override void OnFocus()
    {
        
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (isOn)
        {
            AkSoundEngine.PostEvent("Stop_Radio_RoastBlend", this.gameObject);
            isOn = false;
        }
        else
        {
            AkSoundEngine.PostEvent("Play_Radio_RoastBlend", this.gameObject);
            isOn = true;
        }
    }

    public override void OnLoseFocus()
    {
    }
}
