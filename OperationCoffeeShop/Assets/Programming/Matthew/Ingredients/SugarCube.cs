using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SugarCube : PhysicalIngredient
{
    [SerializeField] private IngredientNode iN;
    private PhysicalIngredient _physicalIngredient;

    private void OnTriggerEnter(Collider other)
    {
        TryAddOrDelete(other.gameObject);
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        this.playerInteraction = playerInteraction;
        playerInteraction.Carry(gameObject);
    }

    private async void TryAddOrDelete(GameObject obj)
    {
        var iC = obj.GetComponent<IngredientContainer>();
        if (!iC) return;
        await iC.AddToContainer(iN, Color.white);
        playerInteraction.DropCurrentObj();
        if(dispenser)
            dispenser.ReleasePoolObject(this);
        else
            Destroy(gameObject);
    }
}