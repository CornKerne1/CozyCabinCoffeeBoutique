using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IngredientContainer : Interactable
{
    [SerializeField] public Transform pourTransform;
    [SerializeField] public GameObject contentsVisualizer;
    public bool inHand;
    public PlayerInteraction pI;
    [SerializeField] public DrinkData dD;
    private float _capacity;
    [SerializeField] private float maxCapacity = 2.0f;
    public bool hasContentsVisualizer = true;
    public float topOfCup;
    private bool _pouring;
    private bool _pouringAction;
    public bool rotating;
    public bool pouringRotation;
    public Dispenser dispenser;
    public bool BigScale;

    public IngredientData iD;

    private IEnumerator _coRef1;
    private IEnumerator _coRef2;

    public List<GameObject> outputIngredients = new List<GameObject>();
    private List<IngredientNode> _garbageList = new List<IngredientNode>();

    private void FixedUpdate()
    {
        HandlePourRotation();
        Pour();
    }

    private IEnumerator Timer(float time)
    {
        _coRef1 = Timer(time);
        yield return new WaitForSeconds(time);
        rotating = false;
        _pouringAction = !_pouringAction;
        _coRef1 = null;
    }

    public void ResetCup()
    {
        _capacity =0;
        outputIngredients = new List<GameObject>();
        contentsVisualizer.transform.localPosition =
            new Vector3(0, 0.0343f, 0);

        contentsVisualizer.transform.localScale =
            new Vector3(5, 5, 5);
    }

    public bool IsPouring()
    {
        return _pouringAction ? _pouringAction : _pouring;
    }

    private void HandlePourRotation()
    {
        if (!rotating) return;
        if (!_pouringAction)
        {
            transform.Rotate(1.85f, 0, 0);
            _pouring = true;
            pouringRotation = true;
            if (_coRef1 != null) return;
            _coRef1 = Timer(1f);
            StartCoroutine(Timer(1f));
        }
        else
        {
            AkSoundEngine.PostEvent("stop_looppour", gameObject);
            _pouring = false;
            transform.Rotate(-1.85f, 0, 0);
            pouringRotation = false;
            if (_coRef1 != null) return;
            _coRef1 = Timer(1f);
            StartCoroutine(Timer(1f));
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
        if (_capacity > maxCapacity)
        {
            IngredientOverflow(iN);
        }
        else
        {
            dD.AddIngredient(iN);
            _capacity = _capacity + iN.target;
            if (hasContentsVisualizer)
            {
                var localPosition = contentsVisualizer.transform.localPosition;
                localPosition = new Vector3(localPosition.x,
                    (localPosition.y - .00048f),
                    localPosition.z);
                contentsVisualizer.transform.localPosition = localPosition;
                if (BigScale)
                {
                    var localScale = contentsVisualizer.transform.localScale;
                    localScale = new Vector3(localScale.x + .01f,
                        (localScale.y + .01f), localScale.z); //
                    contentsVisualizer.transform.localScale = localScale;
                }
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
                case Ingredients.SteamedMilk:
                case Ingredients.FoamedMilk:
                case Ingredients.WhippedCream:
                case Ingredients.UngroundCoffee:
                case Ingredients.GroundCoffee:
                case Ingredients.Salt:
                case Ingredients.EspressoBeans:
                case Ingredients.CoffeeFilter:
                case Ingredients.TeaBag:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    protected virtual void RemoveIngredient()
    {
        foreach (var i in dD.ingredients)
        {
            var c = i.target - 0.1f;
            i.target = c;
            _capacity = _capacity - 0.1f;

            if (i.target <= 0)
            {
                _garbageList.Add(i);
            }
        }

        foreach (var i in _garbageList)
        {
            dD.ingredients.Remove(i);
        }

        _garbageList = new List<IngredientNode>();
    }

    private void Pour()
    {
        if (!_pouring || _coRef2 != null) return;
        _coRef2 = Liquefy();
        StartCoroutine(Liquefy());
    }

    private IEnumerator Liquefy()
    {
        RemoveIngredient();
        yield return new WaitForSeconds(.1f);
        if (outputIngredients.Count > 0)
        {
            if (!GameMode.IsEventPlayingOnGameObject("play_looppour", gameObject))
            {
                AkSoundEngine.PostEvent("play_looppour", gameObject);
            }

            var r = Random.Range(0, outputIngredients.Count);
            Instantiate(outputIngredients[r], pourTransform.position, pourTransform.rotation);
            outputIngredients.Remove(outputIngredients[outputIngredients.Count - 1]);
            if (hasContentsVisualizer)
            {
                var localPosition = contentsVisualizer.transform.localPosition;
                localPosition = new Vector3(localPosition.x,
                    (localPosition.y + .00048f),
                    localPosition.z);
                contentsVisualizer.transform.localPosition = localPosition;
                var localScale = contentsVisualizer.transform.localScale;
                localScale = new Vector3(localScale.x - .01f,
                    (localScale.y - .01f), localScale.z); //
                contentsVisualizer.transform.localScale = localScale;
            }
        }
        else
        {
            AkSoundEngine.PostEvent("stop_looppour", gameObject);
        }

        _coRef2 = null;
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

    private void IngredientOverflow(IngredientNode ingredient)
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
            case Ingredients.SteamedMilk:
            case Ingredients.FoamedMilk:
            case Ingredients.WhippedCream:
            case Ingredients.UngroundCoffee:
            case Ingredients.GroundCoffee:
            case Ingredients.Salt:
            case Ingredients.EspressoBeans:
            case Ingredients.CoffeeFilter:
            case Ingredients.TeaBag:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        if (IsPouring()) return;
        this.pI = interaction;
        interaction.Carry(gameObject);
        inHand = true;
        var rot = new Quaternion(Quaternion.identity.x + rotateOffset.x,
            Quaternion.identity.y + rotateOffset.y,
            Quaternion.identity.z + rotateOffset.z, Quaternion.identity.w);
        transform.rotation = rot;
    }

    public override void OnDrop()
    {
        inHand = false;
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        rotating = true;
    }

    public override void Save(int gameNumber)
    {
        if(delivered)
            gameMode.SaveSystem.SaveGameData.respawnables.Add(new RespawbableData(base.objTypeShop,transform.position,transform.rotation,0));
    }
}