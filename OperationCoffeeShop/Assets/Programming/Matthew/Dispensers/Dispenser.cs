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
    private ObjectPool<GameObject> _pool;


    private Transform _obj;

    public int quantity = 10;
    public bool bottomless;

    public override void Start()
    {
        base.Start();
        ComputerShop.DepositItems += AddItems;
        _pool = new ObjectPool<GameObject>(() =>
            {
                return Instantiate(objType.gameObject);
            }, gameObject =>
            {
                gameObject.SetActive(true);
            },
            gameObject =>
            {
                gameObject.SetActive(false);
                Debug.Log("be free to dissapear" +
                          "");

            },gameObject =>
            {
                Destroy(gameObject);
            }, true, 100, 100);
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

    public void ReleasePoolObject(GameObject obj)
    {
        _pool.Release(obj);
        Debug.Log("We release you coffee bean");
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.playerData.busyHands || (!bottomless && quantity <= 0)) return;
        quantity--;
        UpdateQuantity();
        var _obj = _pool.Get();
        _obj.transform.position = spawnTrans.position;
        _obj.transform.rotation = spawnTrans.rotation;
        if (_obj.gameObject.TryGetComponent<PhysicalIngredient>(out var physicalIngredient))
        {
            physicalIngredient.playerInteraction = playerInteraction;
            physicalIngredient.dispenser = this;
        }
        else if (_obj.gameObject.TryGetComponent<IngredientContainer>(out var ingredientContainer))
        {
            ingredientContainer.pI = playerInteraction;
            ingredientContainer.inHand = true;
        }
        
        playerInteraction.Carry(_obj.gameObject);
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

            if (objType == tuple.Item1)
            {
                this.quantity += tuple.Item2;
                UpdateQuantity();
            }
        }
        catch
        {
            // ignored
        }
    }
}