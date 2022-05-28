using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularCustomer : Customer
{
    private CustomerAI ai;
    [SerializeField, Header("If true ignore next variable")]
    public bool randomTimeOfDay;
    public int spawnTime;

    public void Awake()
    {
        ai = GetComponent<CustomerAI>();
        //CD.DesiredFlavors(CD.favoriteDrink.);
    }
    public override string Dialogue()
    {
        throw new System.NotImplementedException();
    }

    public override DrinkData GetDrinkOrder()
    {
        return CD.orderedDrink;
    }

    public override void NextMove()
    {
        throw new System.NotImplementedException();
    }

    protected override Tree DialogueTree()
    {
        return new Tree();
    }

    protected override IngredientNode GetRandomAddOn()
    {
        throw new System.NotImplementedException();
    }

    protected override DrinkData GetRandomDrink()
    {
        throw new System.NotImplementedException();
    }

    protected override List<Ingredients> GetRandomToppings()
    {
        throw new System.NotImplementedException();
    }
}
