using System;
using System.Collections;
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
    public bool deliveryMode;
    private Transform _obj;
    public int quantity = 10;
    public bool bottomless;

    public override void Start()
    {
        base.Start();
        ComputerShop.DepositItems += AddItems;
        CreatePool();
        StickyNoteSetup();
        if (!deliveryMode)
        {
            TryGetComponent<Rigidbody>(out var rB);
            if(rB)
                rB.isKinematic = true;
        }
        else
            text.transform.parent.gameObject.SetActive(false);
    }

    private void StickyNoteSetup()
    {
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

    private void CreatePool()
    {
        _pool = new ObjectPool<PhysicalIngredient>(
            () => Instantiate(objType.gameObject.GetComponentInChildren<PhysicalIngredient>()),
            physicalIngredient =>
            {
                physicalIngredient.gameObject.SetActive(true);
                physicalIngredient.dispenser = this;
            },
            physicalIngredient => { physicalIngredient.gameObject.SetActive(false); }, physicalIngredient => { physicalIngredient.gameObject.SetActive(false); }, true, 100, 100);
    }

    public void ReleasePoolObject(PhysicalIngredient physicalIngredient)
    {
        _pool.Release(physicalIngredient);
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        playerInteraction = interaction;
        if (deliveryMode)
        {
            if (playerInteraction.carriedObj == gameObject)
                playerInteraction.DropCurrentObj();
            else
                playerInteraction.Carry(this.gameObject);
        }
        else
        {
            TakeFromDispenser();
        }
    }

    private void TakeFromDispenser()
    {
        if (playerInteraction.playerData.busyHands) return;
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
        if (quantity == 0)
            Destroy(gameObject);
    }

    protected void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        if (!deliveryMode) return;
        interaction.DropCurrentObj();
        StartCoroutine(CancelDeliveryMode());
    }

    private IEnumerator CancelDeliveryMode()
    {
        text.transform.parent.gameObject.SetActive(true);
        deliveryMode = false;
        float timeElapsed = 0;
        var rB = GetComponent<Rigidbody>();
        while ((int)rB.velocity.magnitude !=0 ||timeElapsed <=1f)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        GetComponent<Rigidbody>().isKinematic = true;

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

    public override void Load(int gameNumber)
    {
        
    }
    public override void Save(int gameNumber)
    {
       if(delivered)
        gameMode.SaveSystem.SaveGameData.respawnables.Add(new RespawbableData(objTypeShop,transform.position,transform.rotation,quantity));
    }
}