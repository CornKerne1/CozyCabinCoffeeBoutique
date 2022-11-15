using System;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class Dispenser : Interactable
{
    [SerializeField] protected Transform spawnTrans;
    [SerializeField] public ObjectHolder objType;
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected string message = " beans remaining";
    private ObjectPool<PhysicalIngredient> _pool;
    private bool deliveryMode;


    private Transform _obj;

    public int quantity = 10;
    public bool bottomless;

    public override void Start()
    {
        base.Start();
        ComputerShop.DepositItems += AddItems;
        _pool = new ObjectPool<PhysicalIngredient>(
            () => Instantiate(objType.gameObject.GetComponentInChildren<PhysicalIngredient>()),
            physicalIngredient => { physicalIngredient.gameObject.SetActive(true);
                physicalIngredient.dispenser = this;
            },
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

    public override void OnInteract(PlayerInteraction interaction)
    {
        if (deliveryMode)
        {
            if(interaction.carriedObj== gameObject)
                interaction.DropCurrentObj();
            else
                interaction.Carry(this.gameObject);
        }
        else
        {
            if (interaction.playerData.busyHands || (!bottomless && quantity <= 0)) return;
            quantity--;
            UpdateQuantity();
            var ingredient = _pool.Get().transform;
            var transform1 = ingredient.transform;
            transform1.position = spawnTrans.position;
            transform1.rotation = spawnTrans.rotation;
            if (ingredient.gameObject.TryGetComponent<PhysicalIngredient>(out var physicalIngredient))
            {
                physicalIngredient.playerInteraction = interaction;
                physicalIngredient.dispenser = this;
            }
            else if (ingredient.gameObject.TryGetComponent<IngredientContainer>(out var ingredientContainer))
            {
                ingredientContainer.pI = interaction;
                ingredientContainer.inHand = true;
            }

            interaction.Carry(ingredient.gameObject);
            IfTutorial();
        }
    }
    protected void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }

    protected void UpdateQuantity()
    {
        if (text != null)
        {
            text.text = quantity + message;
        }
    }

    protected void AddItems(object sender, EventArgs e)
    {
        try
        {
            var tuple = (Tuple<ObjectHolder, int>)sender;

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