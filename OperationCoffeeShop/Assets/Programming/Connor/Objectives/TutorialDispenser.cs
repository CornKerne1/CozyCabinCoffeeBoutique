using System;
using UnityEngine;
using TMPro;

public class TutorialDispenser : Interactable
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private ObjectHolder objType;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string message = " beans remaining";


    private Transform _obj;

    public int quantity = 10;
    public bool bottomless;

    private Objectives1 _objectives1;
    private bool _inTutorial;
    private bool _completedObjective;

    public override void Start()
    {
        IfTutorial();
        base.Start();
        ComputerShop.DepositItems += AddItems;
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

    private void IfTutorial()
    {
        try
        {
            _objectives1 = GameObject.Find("Objectives").GetComponent<Objectives1>();
            _inTutorial = true;
        }
        catch
        {
            // ignored
        }
    }

    private void IfTutorialObjective()
    {
        if (_inTutorial && ! _completedObjective)
        {
            _objectives1.NextObjective(gameObject);
            _completedObjective = true;
        }
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (playerInteraction.pD.busyHands || (!bottomless && quantity <= 0)) return;
        quantity--;
        UpdateQuantity();
        _obj = Instantiate(objType.gObj, spawnTrans.position, spawnTrans.rotation).transform;
        if (_obj.gameObject.TryGetComponent<PhysicalIngredient>(out var physicalIngredient))
        {
            physicalIngredient.pI = playerInteraction;
        }
        else if (_obj.gameObject.TryGetComponent<IngredientContainer>(out var ingredientContainer))
        {
            ingredientContainer.pI = playerInteraction;
            ingredientContainer.inHand = true;
        }

        playerInteraction.Carry(_obj.gameObject);
        Debug.Log("pick up bean");
        IfTutorialObjective();
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