using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalIngredient : Interactable
{
    private Vector3 rejectionForce = new Vector3(55, 55, 55);
    [SerializeField] public Ingredients thisIngredient;
    bool inHand;
    public PlayerInteraction pI;

    public void Update()
    {
       
    }
    public override void Start()
    {
        base.Start();
        gameObject.tag = "PickUp";
    }



    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        Debug.Log("onInteract");
        this.pI = playerInteraction;
        playerInteraction.Carry(gameObject);
        inHand = true;
    }

    public void OnDrop()
    {
        inHand = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (other.gameObject.layer == 3)
        {
            //pI.DropCurrentObj();
            try
            {
                other.GetComponent<Machine>().IngredientInteract(gameObject);
                rb.AddForce(rejectionForce); //
                pI.DropCurrentObj();

            }
            catch
            {
                try
                {
                    other.GetComponent<BrewerBowl>().IngredientInteract(gameObject);
                    rb.AddForce(rejectionForce);//
                    pI.DropCurrentObj();
                
                }
                catch {}
            }
        }
    }
}
