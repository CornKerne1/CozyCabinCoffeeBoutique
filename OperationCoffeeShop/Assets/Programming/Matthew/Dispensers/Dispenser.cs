using System;
using UnityEngine;
using TMPro;

public class Dispenser : Interactable
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private ObjectHolder objType;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string message = " beans remaining";


    private Transform _obj;

    public int quantity = 10;
    public bool bottomless;

    // Start is called before the first frame update
    public override void Start()
    {
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

    public override void OnInteract(PlayerInteraction pI)
    {
        if (pI.pD.busyHands || (!bottomless && quantity <= 0)) return;
        quantity--;
        UpdateQuantity();
        _obj = Instantiate(objType.gObj, spawnTrans.position, spawnTrans.rotation).transform;
        if (_obj.gameObject.TryGetComponent<PhysicalIngredient>(out var physicalIngredient))
        {
            physicalIngredient.pI = pI;
        }
        else if (_obj.gameObject.TryGetComponent<IngredientContainer>(out var ingredientContainer))
        {
            ingredientContainer.pI = pI;
            ingredientContainer.inHand = true;
        }
        pI.Carry(_obj.gameObject);
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
