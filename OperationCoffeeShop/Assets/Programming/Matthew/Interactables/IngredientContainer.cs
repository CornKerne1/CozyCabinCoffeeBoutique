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

    private bool pouring;
    private bool rotating;

    public IngredientData iD;

    private IEnumerator cr1;

    private void FixedUpdate()
    {
        HandlePourRotation();
    }

    IEnumerator Timer(float time)
    {
        cr1 = Timer(time);
        yield return new WaitForSeconds(time);
        rotating = false;
        pouring = !pouring;
        cr1 = null;
    }
    private void HandlePourRotation()
    {
        if (rotating)
        {
            if (!pouring)
            {
                transform.Rotate(2,0,0);
                if (cr1 == null)
                {
                    StartCoroutine(Timer(1f));
                }
            }
            else
            {
                transform.Rotate(-2,0,0);
                if (cr1 == null)
                {
                    StartCoroutine(Timer(1f));
                }
            }
        }
    }

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
        cr1 = null;
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

    

    public void Pour()
    {
        rotating = true;
    }
    
    public void StopPouring()
    {
        rotating = true;
    }

    void IngredientOverflow(IngredientNode ingredient)
    {
        switch (ingredient.ingredient)
        {
            case Ingredients.BrewedCoffee:
                Instantiate(iD.brewedCoffee, pourTransform.position, pourTransform.rotation);
                break;
            case Ingredients.Espresso:
                Instantiate(iD.espresso, pourTransform.position, pourTransform.rotation);
                break;
        }
    }
    
    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
        pI.Carry(gameObject);
        inHand = true;
        Quaternion rot = new Quaternion(Quaternion.identity.x + rotateOffset.x, Quaternion.identity.y + rotateOffset.y, Quaternion.identity.z + rotateOffset.z, Quaternion.identity.w);
        transform.rotation =rot;
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
        rotating = true;
    }
}
