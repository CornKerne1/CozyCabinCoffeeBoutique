using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalIngredient : Interactable
{
    public IngredientData iD;
    bool inHand;
    PlayerInteraction pI;

    public void Update()
    {
       
    }
    public override void Start()
    {
        base.Start();
        gameObject.tag = "PickUp";
    }

    public override void OnFocus()
    {

    }

    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
        pI.Carry(gameObject);
        inHand = true;
    }

    public void OnDrop()
    {
        inHand = false;
    }

    public override void OnLoseFocus()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (inHand && other.gameObject.layer == 3)
        {
            pI.DropCurrentObj();
            try
            {
                other.GetComponent<Machine>().Interact(gameObject);
            }
            catch { }
        }
    }
}
