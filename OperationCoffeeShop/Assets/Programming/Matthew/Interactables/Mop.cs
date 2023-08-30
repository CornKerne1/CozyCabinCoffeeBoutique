using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mop : Interactable
{
   bool cleaning = false;
   [SerializeField] private Animator animator;
   [SerializeField] private MeshRenderer animatedMesh;
   private Collider _collider;
   private MeshRenderer _mesh;

   public override void Start()
    {
        base.Start();
        _collider=GetComponent<Collider>();
        _mesh = GetComponent<MeshRenderer>();
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
    }
    

    public override void OnDrop()
    {
        base.OnDrop();
        if (cleaning)
            StopCleaning();
    }
    public override void OnAltInteract(PlayerInteraction interaction)
    {
        if (cleaning)
            StopCleaning();
        else
            StartCleaning();
    }
    private void StartCleaning()
    {
        animatedMesh.transform.position = transform.position;
        animatedMesh.transform.rotation = transform.rotation;
        _mesh.enabled = false;
        _collider.enabled = false;
        animatedMesh.enabled = true;
        animator.SetBool("clean",true);
        cleaning = true;
    }
    private void StopCleaning()
    {
        animator.SetBool("clean",false);
        animatedMesh.enabled = false;
        _mesh.enabled = true;
        _collider.enabled = true;
        animatedMesh.transform.position = transform.position;
        animatedMesh.transform.rotation = transform.rotation;
        cleaning = false;
    }

}