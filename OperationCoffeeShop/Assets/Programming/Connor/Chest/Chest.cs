using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    Animator animator;

    public override void OnFocus()
    {
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        animator.SetTrigger("OpenClose");
    }

    public override void OnLoseFocus()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
