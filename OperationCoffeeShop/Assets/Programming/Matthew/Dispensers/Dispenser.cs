using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : Interactable
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private ObjectHolder objType;
    private Transform obj;

    public int quantity = 10;
    public bool bottomless = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (!pI.pD.busyHands && (bottomless || quantity > 0 ))
        {
            quantity--;
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
    public override void OnFocus()
    {
    }
    public override void OnLoseFocus()
    {
    }
}
