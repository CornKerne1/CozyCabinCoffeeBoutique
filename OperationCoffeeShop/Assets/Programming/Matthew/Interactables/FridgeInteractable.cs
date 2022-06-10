using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Serialization;

public class FridgeInteractable : Interactable
{
    private Animator _animator;
    private bool opening;
    private bool open;

    private void Update()
    {
       
    }

    void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (!opening)
        {
            if (open)
            {
                AkSoundEngine.PostEvent("Play_FridgeDoorSqueak", gameObject);
                _animator.SetTrigger("Close");
                StartCoroutine(Timer(1f));
                opening = true;
            }
            else
            {
                AkSoundEngine.PostEvent("Play_FridgeDoorSqueak", gameObject);
                AkSoundEngine.PostEvent("Play_FridgeOpen", gameObject);
                _animator.SetTrigger("Open");
                StartCoroutine(Timer(1f));
                opening = true;
            }
        }
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        opening = false;
        open = !open;
        if (!open)
        {
            AkSoundEngine.PostEvent("Play_FridgeClosed", gameObject);
        }
    }

}
