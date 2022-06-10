using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    private Animator _animator;
    private static readonly int OpenClose = Animator.StringToHash("OpenClose");

    public override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }
    public override void OnFocus()
    {
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        _animator.SetTrigger(OpenClose);
    }

    public override void OnLoseFocus()
    {
    }


}
