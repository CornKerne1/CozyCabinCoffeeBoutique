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
        _animator = GetComponent<Animator>();
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (!opening)
        {
            if (open)
            {
                _animator.SetTrigger("Close");
                StartCoroutine(Timer(1f));
                opening = true;
            }
            else
            {
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
    }
    public override void OnFocus()
    {

    }

    public override void OnLoseFocus()
    {

    }
}
