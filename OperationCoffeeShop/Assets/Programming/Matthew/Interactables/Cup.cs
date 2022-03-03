using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : Interactable
{

    [SerializeField] public DrinkData dD;

    public override void Awake()
    {
        base.Awake();
        dD = (DrinkData)ScriptableObject.CreateInstance("DrinkData");
        

    }

    public override void Start()
    {
        base.Start();
        dD.Ingredients = new List<IngredientNode>();
        dD.Name = "Cup";
        dD.addIngredient(new IngredientNode(Ingredients.Milk, 100000f));
    }

    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }



    public override void OnInteract(PlayerInteraction pI)
    {
        Debug.Log("SOMETHING");
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }
}
