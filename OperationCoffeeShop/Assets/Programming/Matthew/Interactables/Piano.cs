using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : Interactable
{
    bool on = false;

    Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public override void OnInteract(PlayerInteraction pI)
    {
        if (!GameMode.IsEventPlayingOnGameObject("Play_Piano", this.gameObject))
        {
            on = true;
            AkSoundEngine.PostEvent("Play_Piano", this.gameObject);
            animator.SetBool("On", true);
        }
        else if (on)
        {
            on = false;
            AkSoundEngine.PostEvent("VolumeZero_Piano", this.gameObject);
            animator.SetBool("On",false);
        }
        else
        {
            on = true;
            AkSoundEngine.PostEvent("VolumeOne_Piano", this.gameObject);
            animator.SetBool("On",true);

        }
    }
}
