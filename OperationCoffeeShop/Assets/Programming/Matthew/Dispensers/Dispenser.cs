using System;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class Dispenser : Interactable
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] public ObjectHolder objType;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string message = " beans remaining";
    private ObjectPool<PhysicalIngredient> _pool;


    private Transform _obj;

    public int quantity = 10;
    public bool bottomless;

    public override void Start()
    {
        base.Start();
        ComputerShop.DepositItems += AddItems;
        _pool = new ObjectPool<PhysicalIngredient>(
            () => Instantiate(objType.gameObject.GetComponentInChildren<PhysicalIngredient>()),
            physicalIngredient => { physicalIngredient.gameObject.SetActive(true); },
            physicalIngredient => { physicalIngredient.gameObject.SetActive(false); }, Destroy, true, 100, 100);
        if (bottomless) return;
        try
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
            if (!bottomless)
                text.text = quantity + message;
            else text.text = "Bottomless";
        }
        catch
        {
            text = null;
        }
    }

    public void ReleasePoolObject(PhysicalIngredient physicalIngredient)
    {
        _pool.Release(physicalIngredient);
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.playerData.busyHands || (!bottomless && quantity <= 0)) return;
        quantity--;
        UpdateQuantity();
        var ingredient = _pool.Get().transform;
        var transform1 = ingredient.transform;
        transform1.position = spawnTrans.position;
        transform1.rotation = spawnTrans.rotation;
        if (ingredient.gameObject.TryGetComponent<PhysicalIngredient>(out var physicalIngredient))
        {
            physicalIngredient.playerInteraction = playerInteraction;
            physicalIngredient.dispenser = this;
        }
        else if (ingredient.gameObject.TryGetComponent<IngredientContainer>(out var ingredientContainer))
        {
            ingredientContainer.pI = playerInteraction;
            ingredientContainer.inHand = true;
        }

        playerInteraction.Carry(ingredient.gameObject);
        IfTutorial();
    }

    private void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }

    private void UpdateQuantity()
    {
        if (text != null)
        {
            text.text = quantity + message;
        }
    }

    private void AddItems(object sender, EventArgs e)
    {
        try
        {
            Tuple<ObjectHolder, int> tuple = (Tuple<ObjectHolder, int>)sender;

            if (objType != tuple.Item1) return;
            this.quantity += tuple.Item2;
            UpdateQuantity();
        }
        catch
        {
            // ignored
        }
    }
}