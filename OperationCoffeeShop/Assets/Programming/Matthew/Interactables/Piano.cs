using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : Interactable
{
    public override void OnInteract(PlayerInteraction pI)
    {
        if (!GameMode.IsEventPlayingOnGameObject("Play_Piano", this.gameObject))
            AkSoundEngine.PostEvent("Play_Piano", this.gameObject);
    }

    public override void OnFocus()
    {
    }

    public override void OnLoseFocus()
    {
    }
}
