using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SugarCube : PhysicalIngredient
{
    [SerializeField] private IngredientNode iN;
    private PhysicalIngredient _physicalIngredient;
    private bool _hasCollided;

    private void OnTriggerEnter(Collider other)
    {
        TryAddOrDelete(other.gameObject);
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        this.playerInteraction = playerInteraction;
        playerInteraction.Carry(gameObject);
    }

    private void TryAddOrDelete(GameObject obj)
    {
        if (_hasCollided) return;
        var ingredientContainer = obj.GetComponent<IngredientContainer>();
        if (!ingredientContainer) return;
        playerInteraction.DropCurrentObj();
        ingredientContainer.AddToContainer(
            iN); //WRITE CODE THAT CHECKS IF THIS INGREDIENT IS ALREADY ON LIST. IF SO ONLY USE THE AMOUNT AND DONT ADD THE ARRAY ELEMENT;
        _hasCollided = true;
        dispenser.ReleasePoolObject(this);
        }
}