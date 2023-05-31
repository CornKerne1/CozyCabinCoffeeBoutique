using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class IngredientContainer : Interactable
{
    [SerializeField] public Transform pourTransform;
    [SerializeField] public GameObject contentsVisualizer;
    [SerializeField] public GameObject steam;
    private Vector3 _visualizerStartPosition;
    [SerializeField] private float visualizerStartScale,visualizerFullScale;
    public bool inHand;
    public PlayerInteraction pI;
    [SerializeField] public DrinkData dD;
    [SerializeField] private float maxCapacity = 1f;
    public bool hasContentsVisualizer = true;
    private Material _visualizerMaterial;
    [SerializeField] private float bottomOfCup,topOfCup;
    private bool _pouring,_pouringAction;
    public bool rotating,pouringRotation;
    [SerializeField]private bool scaleVisualizer;

    public IngredientAtlas iD;

    private Task _task1Running;
    private Task _task2Running;

    public List<GameObject> outputIngredients = new List<GameObject>();
    [SerializeField] private List<IngredientNode> garbageList = new List<IngredientNode>();
    private static readonly int ColorLight = Shader.PropertyToID("_Color"),ColorLightBubbles = Shader.PropertyToID("_ColorLightBubbles");
    private static readonly int Alpha = Shader.PropertyToID("Alpha");
    private bool _waterColor;

    private async void FixedUpdate()
    {
        HandlePourRotation();
        if (rotating) return;
        Pour();
    }

    private async Task Timer(float time)
    {
        await Task.Delay(TimeSpan.FromSeconds(time));
        rotating = false;
        _pouringAction = !_pouringAction;
        _task1Running = null;
    }

    public void ResetCup()
    {
        if (steam)steam.SetActive(false);
        outputIngredients = new List<GameObject>();
        if (contentsVisualizer)
        {
            contentsVisualizer.transform.localPosition =
                _visualizerStartPosition;
            contentsVisualizer.transform.localScale =
                new Vector3(visualizerStartScale, visualizerStartScale, visualizerStartScale);
        }
        _visualizerMaterial.SetColor(ColorLightBubbles,Color.clear);
    }

    public bool IsPouring()
    {
        return _pouringAction ? _pouringAction : _pouring;
    }

    private async void HandlePourRotation()
    {
        if (!rotating) return;
        if (!_pouringAction)
        {
            transform.Rotate(6.75f, 0, 0);
            _pouring = true;
            pouringRotation = true;
            if (_task1Running != null) return;
            _task1Running = Timer(.5f);
            await _task1Running;
        }
        else
        {
            AkSoundEngine.PostEvent("stop_looppour", gameObject);
            _pouring = false;
            transform.Rotate(-6.75f, 0, 0);
            pouringRotation = false;
            garbageList = new List<IngredientNode>();
            if (_task1Running != null) return;
            _task1Running = Timer(.5f);
            await _task1Running;
        }
    }

    public override void Awake()
    {
        if (!hasContentsVisualizer)
        {
            Destroy(contentsVisualizer);
        }
        else
        {
            _visualizerMaterial = contentsVisualizer.GetComponent<MeshRenderer>().material;
            _visualizerMaterial.SetColor(ColorLightBubbles, Color.clear);
            _visualizerStartPosition = contentsVisualizer.transform.localPosition;
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
        if (contentsVisualizer)
            _visualizerMaterial.SetFloat(Alpha, 0);
    }

    public virtual async void AddToContainer(IngredientNode iN)
    {
        await AddToContainer(iN, Color.clear);
    }

    private float GetCapacity()
    {
        float capacity = 0;
        foreach (var i in dD.ingredients)
        {
            capacity = capacity + i.target;
        }
        return capacity;
    }
    public virtual async Task AddToContainer(IngredientNode iN, Color color)
    {
        if (GetCapacity() >= maxCapacity)
        {
            await IngredientOverflow(iN);
            return;
        }
        if (_waterColor && iN.ingredient != Ingredients.Water)
        {
            _visualizerMaterial.SetColor(ColorLightBubbles, Color.clear);
            _waterColor = false;
        }
        else
        {
            var _color = color;
            dD.AddIngredient(iN);
            switch (iN.ingredient)
            {
                case Ingredients.BrewedCoffee:
                    outputIngredients.Add(iD.brewedCoffee);
                    if (!steam||steam.activeSelf) break;
                    steam.SetActive(true);
                    break;
                case Ingredients.Espresso:
                    outputIngredients.Add(iD.espresso);
                    if (!steam||steam.activeSelf) break;
                    steam.SetActive(true);
                    break;
                case Ingredients.Milk:
                    outputIngredients.Add(iD.milk);
                    break;
                case Ingredients.Sugar:
                    outputIngredients.Add(iD.Sugar);
                    break;
                case Ingredients.Water:
                    outputIngredients.Add(iD.water);
                    if (GetCapacity() > .01f)
                        _color = Color.clear;
                    else
                    {
                        _waterColor = true;
                    }

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
                    break;

            }
            if (hasContentsVisualizer)
            {
                float fillPercentage = (float)GetCapacity() / maxCapacity;
                if (_visualizerMaterial.GetColor(ColorLightBubbles) == Color.clear)
                {
                    _visualizerMaterial.SetColor(ColorLightBubbles, _color);
                    _visualizerMaterial.SetFloat(Alpha,_visualizerMaterial.GetFloat(Alpha)+.01f);
                }
                else
                {
                    var newColor = Color.Lerp(_visualizerMaterial.GetColor(ColorLightBubbles), _color, .01f);
                    _visualizerMaterial.SetColor(ColorLightBubbles,newColor);
                    _visualizerMaterial.SetFloat(Alpha,_visualizerMaterial.GetFloat(Alpha)+.01f);
                }
                if (_visualizerMaterial.GetFloat(Alpha) > 1)
                {
                    _visualizerMaterial.SetFloat(Alpha,1);
                }
                float newYPosition = Mathf.Lerp(bottomOfCup, topOfCup, fillPercentage);
                var localPosition = new Vector3(contentsVisualizer.transform.localPosition.x, newYPosition, contentsVisualizer.transform.localPosition.z);
                contentsVisualizer.transform.localPosition = localPosition;
                if (scaleVisualizer)
                {
                    float newScale = Mathf.Lerp(visualizerStartScale, visualizerFullScale, fillPercentage);
                    Vector3 localScale = new Vector3(newScale, newScale, newScale);
                    contentsVisualizer.transform.localScale = localScale;
                }
            }
        }
    }

    protected virtual async void RemoveIngredient()
    {
        if (dD.ingredients.Count <= 0) return;
        if (contentsVisualizer)
        {
            _visualizerMaterial.SetFloat(Alpha,_visualizerMaterial.GetFloat(Alpha)-.01f);
            if (_visualizerMaterial.GetFloat(Alpha) < 0)
            {
                _visualizerMaterial.SetFloat(Alpha, 0);
            }
        }
        var iN = dD.ingredients[^1];
        switch (iN.ingredient)
        {
            case Ingredients.BrewedCoffee:
                iN.target = iN.target - .01f;
                break;
            case Ingredients.Espresso:
                iN.target = iN.target - .01f;
                break;
            case Ingredients.Milk:
                iN.target = iN.target - .01f;
                break;
            case Ingredients.Sugar:
                iN.target = iN.target - .05f;
                break;
            case Ingredients.Water:
                iN.target = iN.target - .01f;
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
            case Ingredients.Vanilla:
            case Ingredients.Mocha:
            default:
                break;
        }
        if (!(iN.target <= 0)) return;
        garbageList.Add(iN);
        dD.ingredients.Remove(iN);
    }

    private async void Pour()
    {
        if (!_pouring || _task2Running != null) return;
        _task2Running = Liquefy();
        await _task2Running;
    }

    private async Task Liquefy()
    {
        RemoveIngredient();
        await Task.Delay(75);
        if (outputIngredients.Count > 0)
        {
            if (!GameMode.IsEventPlayingOnGameObject("play_looppour", gameObject))
            {
                AkSoundEngine.PostEvent("play_looppour", gameObject);
            }

            var r = Random.Range(0, outputIngredients.Count);
            Instantiate(outputIngredients[r], pourTransform.position, pourTransform.rotation);
            outputIngredients.Remove(outputIngredients[outputIngredients.Count - 1]);
            if (hasContentsVisualizer &&GetCapacity()>0)
            {
                float fillPercentage = (float)GetCapacity() / maxCapacity;
                float newYPosition = Mathf.Lerp(bottomOfCup, topOfCup, fillPercentage);
                var localPosition = new Vector3(contentsVisualizer.transform.localPosition.x, newYPosition, contentsVisualizer.transform.localPosition.z);
                contentsVisualizer.transform.localPosition = localPosition;
                if (scaleVisualizer)
                {
                    float newScale = Mathf.Lerp(visualizerStartScale, visualizerFullScale, fillPercentage);
                    Vector3 localScale = new Vector3(newScale, newScale, newScale);
                    contentsVisualizer.transform.localScale = localScale;
                }
            }
            else if(hasContentsVisualizer)
            {
                _visualizerMaterial.SetColor(ColorLightBubbles, Color.clear);
                _visualizerMaterial.SetFloat(Alpha, 0);
                ResetCup();
            }
        }
        else
        {
            ResetCup();
            AkSoundEngine.PostEvent("stop_looppour", gameObject);
        }

        _task2Running = null;
    }

    public async void StopPouring()
    {
        await WaitForPour();
    }

    private async Task WaitForPour()
    { 
        await Task.Delay(1000);
        if (IsPouring())
        {
            rotating = true;
        }
    }

    private async Task IngredientOverflow(IngredientNode ingredient)
    {
        await Task.Delay(4);
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
            case Ingredients.Water:
                Instantiate(iD.water, pourTransform.position, pourTransform.rotation);
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