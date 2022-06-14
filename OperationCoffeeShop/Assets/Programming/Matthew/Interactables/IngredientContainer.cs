using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IngredientContainer : Interactable
{
    [SerializeField] public Transform pourTransform;
    [SerializeField] public GameObject contentsVisualizer;
    public bool inHand;
    public PlayerInteraction pI;
    [SerializeField] public DrinkData dD;
    private float capacity;
    [SerializeField] private float maxCapacity = 2.0f;
    public bool hasContentsVisualizer = true;
    public float topOfCup;
    private bool pouring;
    private bool pouringAction;
    public bool rotating;
    public bool pouringRotation;

    public IngredientData iD;

    private IEnumerator cr1 = null;
    private IEnumerator cr2 = null;

    public List<GameObject> outputIngredients = new List<GameObject>();
    private List<IngredientNode> garbageList = new List<IngredientNode>();

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
                pouringRotation = true;
                if (cr1 == null)
                {
                    cr1 = Timer(1f);
                    StartCoroutine(Timer(1f));
                }
            }
            else
            {
                pouring = false;
                transform.Rotate(-2, 0, 0);
                pouringRotation = false;
                if (cr1 == null)
                {
                    cr1 = Timer(1f);
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
        dD.ingredients = new List<IngredientNode>();
        dD.name = "Cup";
    }

    public virtual void AddToContainer(IngredientNode iN)
    {
        if (capacity > maxCapacity)
        {
            IngredientOverflow(iN);
        }
        else
        {
            dD.AddIngredient(iN);
            capacity = capacity + iN.target;
            if (hasContentsVisualizer)
            {
                contentsVisualizer.transform.localPosition = new Vector3(contentsVisualizer.transform.localPosition.x,
                    (contentsVisualizer.transform.localPosition.y - .00048f),
                    contentsVisualizer.transform.localPosition.z);
                contentsVisualizer.transform.localScale = new Vector3(contentsVisualizer.transform.localScale.x + .01f,
                    (contentsVisualizer.transform.localScale.y + .01f), contentsVisualizer.transform.localScale.z); //
            }

            switch (iN.ingredient)
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
                case Ingredients.Sugar:
                    outputIngredients.Add(iD.Sugar);
                    break;
            }
        }
    }

    public virtual void RemoveIngredient()
    {
        foreach (IngredientNode i in dD.ingredients)
        {
            var c = i.target - 0.1f;
            i.target = c;
            capacity = capacity - 0.1f;

            if (i.target <= 0)
            {
                garbageList.Add(i);
            }
        }

        foreach (IngredientNode i in garbageList)
        {
            dD.ingredients.Remove(i);
        }

        garbageList = new List<IngredientNode>();
        //Queue Each Ingriedent for spawning using a list, then use a coroutine to spawn each ingriedent with a small buffer.
    }

    public void Pour()
    {
        if (pouring && cr2 == null)
        {
            cr2 = Liquify();
            StartCoroutine(Liquify());
        }
    }

    IEnumerator Liquify()
    {
        RemoveIngredient();
        yield return new WaitForSeconds(.04f);
        if (outputIngredients.Count > 0)
        {
            var r = Random.Range(0, outputIngredients.Count);
            Instantiate(outputIngredients[r], pourTransform.position, pourTransform.rotation);
            outputIngredients.Remove(outputIngredients[outputIngredients.Count - 1]);
            if (hasContentsVisualizer)
            {
                contentsVisualizer.transform.localPosition = new Vector3(contentsVisualizer.transform.localPosition.x,
                    (contentsVisualizer.transform.localPosition.y + .00048f),
                    contentsVisualizer.transform.localPosition.z);
                contentsVisualizer.transform.localScale = new Vector3(contentsVisualizer.transform.localScale.x - .01f,
                    (contentsVisualizer.transform.localScale.y - .01f), contentsVisualizer.transform.localScale.z); //
            }
        }

        cr2 = null;
    }

    public void StopPouring()
    {
        StartCoroutine(WaitForPour());
    }

    IEnumerator WaitForPour()
    {
        yield return new WaitForSeconds(1f);
        if (IsPouring())
        {
            rotating = true;
        }
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
            case Ingredients.Sugar:
                Instantiate(iD.Sugar, pourTransform.position, pourTransform.rotation);
                break;
            case Ingredients.Milk:
                Instantiate(iD.milk, pourTransform.position, pourTransform.rotation);
                break;
        }
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (!IsPouring())
        {
            this.pI = pI;
            pI.Carry(gameObject);
            inHand = true;
            Quaternion rot = new Quaternion(Quaternion.identity.x + rotateOffset.x,
                Quaternion.identity.y + rotateOffset.y,
                Quaternion.identity.z + rotateOffset.z, Quaternion.identity.w);
            transform.rotation = rot;
        }
    }

    public void OnDrop()
    {
        inHand = false;
    }

    public override void OnAltInteract(PlayerInteraction pI)
    {
        rotating = true;
    }
}