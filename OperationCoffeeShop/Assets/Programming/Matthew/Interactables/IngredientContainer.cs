using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IngredientContainer : Interactable
{
    [SerializeField] public Transform pourTransform;
    [SerializeField] public GameObject contentsVisualizer;
    public bool inHand;
    public PlayerInteraction pI;
    [SerializeField] public DrinkData dD;
    private float capacity;
    private float maxCapacity = 2.0f;
    public bool hasContentsVisualizer = true;
    public float topOfCup;
    private Quaternion startingRotation;
    
    [SerializeField]private bool pouring;

    public IngredientData iD;

    public override void Awake()
    {
        if (!hasContentsVisualizer)
        {
            Destroy(contentsVisualizer);
        }
        base.Awake();
        dD = (DrinkData)ScriptableObject.CreateInstance("DrinkData");
        

    }

    public override void Start()
    {
        base.Start();
        gameObject.tag = "PickUp";
        dD.Ingredients = new List<IngredientNode>();
        dD.Name = "Cup";
        startingRotation = transform.localRotation;
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
            contentsVisualizer.transform.localPosition = new Vector3(contentsVisualizer.transform.localPosition.x, (contentsVisualizer.transform.localPosition.y - .00048f) ,contentsVisualizer.transform.localPosition.z);
            contentsVisualizer.transform.localScale = new Vector3(contentsVisualizer.transform.localScale.x + .01f, (contentsVisualizer.transform.localScale.y + .01f) ,contentsVisualizer.transform.localScale.z);
            
        }


    }

    public void Update()
    {
    }
    

    public void Pour()
    {
        
    }

    void IngredientOverflow(IngredientNode ingredient)
    {
        switch (ingredient.ingredient)
        {
            case Ingredients.BrewedCoffee:
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

    public override void OnAltInteract(PlayerInteraction pI)
    {
        Pour();
    }

    public override void OnAltInteractCanceled()
    {
        
    }
}
