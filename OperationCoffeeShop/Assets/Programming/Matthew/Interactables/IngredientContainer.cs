using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientContainer : Interactable
{
    bool inHand;
    public PlayerInteraction pI;
    [SerializeField] public DrinkData dD;

    public override void Awake()
    {
        base.Awake();
        dD = (DrinkData)ScriptableObject.CreateInstance("DrinkData");
        

    }

    public override void Start()
    {
        base.Start();
        gameObject.tag = "PickUp";
        dD.Ingredients = new List<IngredientNode>();
        dD.Name = "Cup";
    }

    public virtual void AddToContainer(IngredientNode iN)
    {
        dD.addIngredient(iN);
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
        pI.Carry(gameObject);
        inHand = true;
    }

    public override void OnFocus()
    {

    }

    public void OnDrop()
    {
        inHand = false;
    }

    public override void OnLoseFocus()
    {

    }
    
}
