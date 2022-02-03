using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularCustomer : Customer
{
    public override string Dialogue()
    {
        throw new System.NotImplementedException();
    }

    public override DrinkData GetDrinkOrder()
    {
        throw new System.NotImplementedException();
    }

    public override void NextMove()
    {
        throw new System.NotImplementedException();
    }

    protected override Tree DialogueTree()
    {
        throw new System.NotImplementedException();
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
