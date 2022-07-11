using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SlabInteractable : Interactable
{
    public SlabData slabData;
    public Slot slot;
    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        playerInteraction.Carry(gameObject);
    }
}
