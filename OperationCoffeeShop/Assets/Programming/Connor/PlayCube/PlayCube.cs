using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCube : MachineInteraction
{
    public Animator PlayCubeAnimator;

    string isOpen = "isOpen";

    private bool canOpen = true;
    
    public override void OnInteract(PlayerInteraction pI)
    {
        StartCoroutine(interact());
    }

    IEnumerator interact()
    {
        if (canOpen)
        {
            canOpen = false;
            if (PlayCubeAnimator.GetBool(isOpen) == true)
            {
                PlayCubeAnimator.SetBool(isOpen, false);
            }
            else
            {
                PlayCubeAnimator.SetBool(isOpen, true);
            }
            yield return new WaitForSeconds(1);
            canOpen = true;
        }
        yield return null;
    }


    
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

    }


}
