using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomCustomer : Customer
{
    override public string Name()
    {
        throw new NotImplementedException();
    }

    protected override List<Ingredients> GetRandomAddOns()
    {
        throw new NotImplementedException();
    }

    
    protected override Drinks GetRandomDrink()
    {
        throw new NotImplementedException();
    }

    public void compareingredients()
    {
        List<GameObject> newList = new List<GameObject>();
        
      // int i = newList.Find(g);
    }

    protected override List<Ingredients> GetRandomToppings()
    {
        throw new NotImplementedException();
    }

    protected override Tree DialogueTree()
    {
        throw new NotImplementedException();
    }

    public override string Dialogue()
    {
        throw new NotImplementedException();
    }

    public override void NextMove()
    {
        throw new NotImplementedException();
    }
}
