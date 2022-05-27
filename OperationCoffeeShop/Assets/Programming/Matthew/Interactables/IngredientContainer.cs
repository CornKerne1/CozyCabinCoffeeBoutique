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
    private bool pouringAction;
    private bool rotating;

    public IngredientData iD;

    private IEnumerator cr1 = null;
    private IEnumerator cr2 = null;

    public List<GameObject> outputIngredients = new List<GameObject>();

    private void FixedUpdate()
    {
        HandlePourRotation();
        Pour();
    }

    IEnumerator Timer(float time)
    {
        cr1 = Timer(time);
        yield return new WaitForSeconds(time);
        rotating = false;
        pouringAction = !pouringAction;
        cr1 = null;
    }

    public bool IsPouring()
    {
        if (pouringAction)
            return pouringAction;
        else
            return pouring;
    }

    private void HandlePourRotation()
    {
        if (rotating)
        {
            if (!pouringAction)
                {
                    transform.Rotate(2, 0, 0);
                    pouring = true;
                    if (cr1 == null)
                    {
                        StartCoroutine(Timer(1f));
                    }
                }
                else
                {
                    pouring = false;
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
        cr2 = null;
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
            contentsVisualizer.transform.localScale = new Vector3(contentsVisualizer.transform.localScale.x + .01f, (contentsVisualizer.transform.localScale.y + .01f) ,contentsVisualizer.transform.localScale.z);//
            
        }


    }

    public virtual void RemoveIngredient()
    {
        foreach (IngredientNode i in dD.Ingredients)
        {
            i.target = i.target - 0.1f;
            capacity = capacity - 0.1f;
            switch (i.ingredient)
            {
                case Ingredients.BrewedCoffee:
                    outputIngredients.Add(iD.brewedCoffee);
                    break;
                case Ingredients.Espresso:
                    outputIngredients.Add(iD.espresso);
                    break;
                case Ingredients.Milk:
                    outputIngredients.Add(iD.milk);
                    break;
            }
            if (i.target <= 0)
            {
                dD.Ingredients.Remove(i);
            }
        }
            //Queue Each Ingriedent for spawning using a list, then use a coroutine to spawn each ingriedent with a small buffer.
    }
    public void Pour()
    {
        if (pouring && outputIngredients.Count > 0)
            if (cr2 != null)
                StartCoroutine(Liquify());
    }

    IEnumerator Liquify()
    {
        cr2 = Liquify();
        yield return new WaitForSeconds(.04f);
        Instantiate(outputIngredients[outputIngredients.Count], pourTransform.position, pourTransform.rotation);
        cr2 = null;
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
