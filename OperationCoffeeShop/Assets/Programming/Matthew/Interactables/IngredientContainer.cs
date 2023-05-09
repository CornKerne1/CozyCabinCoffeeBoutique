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
    private Vector3 _visualizerStartPosition;
    [SerializeField] private Vector3 visualizerStartScale= new Vector3(5, 5, 5);
    [SerializeField] public float visualizerPositionIncrement = .00048f;
    public bool inHand;
    public PlayerInteraction pI;
    [SerializeField] public DrinkData dD;
    private int _capacity;
    [SerializeField] private float maxCapacity = 100f;
    public bool hasContentsVisualizer = true;
    private Material _visualizerMaterial;
    public float topOfCup;
    private bool _pouring,_pouringAction;
    public bool rotating,pouringRotation;
    [FormerlySerializedAs("BigScale")][SerializeField]private bool scaleVisualizer;

    public IngredientData iD;

    private Task _task1Running;
    private Task _task2Running;

    public List<GameObject> outputIngredients = new List<GameObject>();
    private List<IngredientNode> _garbageList = new List<IngredientNode>();
    private static readonly int ColorDark = Shader.PropertyToID("_ColorDark"),ColorLight = Shader.PropertyToID("_ColorLight");
    private static readonly int Alpha = Shader.PropertyToID("Alpha");
    private float visualizerScaleIncrement = .02f;

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
        _capacity =0;
        outputIngredients = new List<GameObject>();
        contentsVisualizer.transform.localPosition =
            _visualizerStartPosition;

        contentsVisualizer.transform.localScale =
            visualizerStartScale;
        _visualizerMaterial.SetColor(ColorDark,Color.clear);
        _visualizerMaterial.SetColor(ColorLight,Color.clear);
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
            transform.Rotate(3.7f, 0, 0);
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
            transform.Rotate(-3.7f, 0, 0);
            pouringRotation = false;
            _garbageList = new List<IngredientNode>();
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
            _visualizerMaterial.SetColor(ColorDark, Color.clear);
            _visualizerMaterial.SetColor(ColorLight, Color.clear);
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
    }

    public virtual void AddToContainer(IngredientNode iN)
    {
        AddToContainer(iN, Color.clear);
    }
    public virtual void AddToContainer(IngredientNode iN, Color color)
    {
        Debug.Log(_capacity);
        if (_capacity >= maxCapacity)
        {
            IngredientOverflow(iN);
        }
        else
        {
            dD.AddIngredient(iN);
            _capacity = _capacity + 1;
            if (hasContentsVisualizer)
            {
                if (_visualizerMaterial.GetColor(ColorDark) == Color.clear)
                {
                    _visualizerMaterial.SetColor(ColorDark, color);
                    _visualizerMaterial.SetColor(ColorLight, color);
                    _visualizerMaterial.SetFloat(Alpha,1);
                }
                else
                {
                    var newColor = Color.Lerp(_visualizerMaterial.GetColor(ColorDark), color, .01f);
                    _visualizerMaterial.SetColor(ColorDark,newColor);
                    _visualizerMaterial.SetColor(ColorLight,newColor);
                }
                var localPosition = contentsVisualizer.transform.localPosition;
                localPosition = new Vector3(localPosition.x,
                    (localPosition.y - visualizerPositionIncrement),
                    localPosition.z);
                contentsVisualizer.transform.localPosition = localPosition;
                if (scaleVisualizer)
                {
                    var localScale = contentsVisualizer.transform.localScale;
                    localScale = new Vector3(localScale.x + visualizerScaleIncrement,
                        (localScale.y + visualizerScaleIncrement), localScale.z); //
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
                case Ingredients.Water:
                    outputIngredients.Add(iD.water);
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

    protected virtual async void RemoveIngredient()
    {
        if (dD.ingredients.Count <= 0) {_capacity = _capacity - 1;return;}
        var iN = dD.ingredients[dD.ingredients.Count - 1];
        iN.target = iN.target - 0.01f;
        _capacity = _capacity - 1;
        if (iN.target <= 0)
        {
            _garbageList.Add(iN);
        }

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
            if (hasContentsVisualizer &&_capacity>0)
            {
                var localPosition = contentsVisualizer.transform.localPosition;
                localPosition = new Vector3(localPosition.x,
                    (localPosition.y + visualizerPositionIncrement),
                    localPosition.z);
                contentsVisualizer.transform.localPosition = localPosition;
                if (scaleVisualizer)
                {
                    var localScale = contentsVisualizer.transform.localScale;
                    localScale = new Vector3(localScale.x - visualizerScaleIncrement,
                        (localScale.y - visualizerScaleIncrement), localScale.z); //
                    contentsVisualizer.transform.localScale = localScale;
                }
            }
            else if(hasContentsVisualizer)
            {
                _visualizerMaterial.SetColor(ColorDark, Color.clear);
                _visualizerMaterial.SetColor(ColorLight, Color.clear);
                _visualizerMaterial.SetFloat(Alpha, 0);
                _capacity = 0;

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
            case Ingredients.Water:
                outputIngredients.Add(iD.water);
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