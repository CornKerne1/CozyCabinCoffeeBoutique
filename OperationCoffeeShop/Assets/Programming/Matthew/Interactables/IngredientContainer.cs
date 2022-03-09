using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IngredientContainer : Interactable
{
    [SerializeField] public Transform pourTransform;
    bool inHand;
    public PlayerInteraction pI;
    [SerializeField] public DrinkData dD;
    private float capacity;
    private float maxCapacity = 2.0f;
    
    public IngredientData iD;

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
        if (capacity > maxCapacity)
        {
            IngredientOverflow(iN);
        }
        else
        {
            dD.addIngredient(iN);
            capacity = capacity + iN.target;
        }


    }

    void IngredientOverflow(IngredientNode ingredient)
    {
        switch (ingredient.ingredient)
        {
            case Ingredients.EspressoShot:
                Instantiate(iD.brewedCoffee, pourTransform.position, pourTransform.rotation);
                break;
        }
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
