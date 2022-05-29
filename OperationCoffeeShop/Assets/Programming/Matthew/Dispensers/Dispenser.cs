using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dispenser : Interactable
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private ObjectHolder objType;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string message = " beans remaing";


    private Transform obj;

    public int quantity = 10;
    public bool bottomless = false;

    // Start is called before the first frame update
    void Start()
    {
        ComputerShop.DepositItems += AddItems;
        if (!bottomless)
            try
            {
                text = GetComponentInChildren<TextMeshProUGUI>();
                if (!bottomless)
                    text.text = quantity + message;
                else text.text = "Botomless";

            }
            catch
            {
                text = null;
            }


    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (!pI.pD.busyHands && (bottomless || quantity > 0))
        {
            quantity--;
            updateQuantity();
            obj = Instantiate(objType.gObj, spawnTrans.position, spawnTrans.rotation).transform;
            PhysicalIngredient phyIng;
            IngredientContainer ingCon;
            if (obj.gameObject.TryGetComponent<PhysicalIngredient>(out phyIng))
            {
                phyIng.pI = pI;
            }
            else if (obj.gameObject.TryGetComponent<IngredientContainer>(out ingCon))
            {
                ingCon.pI = pI;
                ingCon.inHand = true;
            }
            pI.Carry(obj.gameObject);
        }
    }

    private void updateQuantity()
    {
        if (text != null)
        {
            text.text = quantity + message;
        }
    }
    public void AddItems(object sender, EventArgs e)
    {
        Tuple<ObjectHolder, int> tuple= (Tuple<ObjectHolder,int>)sender;

        if (objType == tuple.Item1)
        {
            this.quantity += tuple.Item2;
            updateQuantity();
        }
    }




    public override void OnFocus()
    {
    }
    public override void OnLoseFocus()
    {
    }
}
