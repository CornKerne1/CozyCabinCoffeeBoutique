using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomCustomer : Customer
{
    [SerializeField]
    private RandomNameSet nameSet;

    private string customerName;

    System.Random random = new System.Random();
    RandomCustomer()
    {
        //sets a random name from nameSet
        customerName = nameSet.names[random.Next(nameSet.names.Count - 1)];
    }
    RandomCustomer(RandomNameSet nameSet)
    {
        //Uses specified nameset to generate random name
        this.nameSet = nameSet;
        //sets a random name from nameSet
        customerName = nameSet.names[random.Next(nameSet.names.Count - 1)];
    }

    override public string GetName()
    {
        return customerName;
    }

    protected override IngredientNode GetRandomAddOn()
    {
        int ingredient = random.Next(PRD.learnedIngredients.Count - 1);
        float target = random.Next(0, 1);
        return new IngredientNode(PRD.learnedIngredients[ingredient],target);
    }

    protected override DrinkData GetRandomDrink()
    {
      
        return PRD.learnedDrinks[random.Next(PRD.learnedIngredients.Count - 1)];
    }
    public override DrinkData GetDrinkOrder()
    {
        IngredientNode addOn = GetRandomAddOn();
        DrinkData drink = GetRandomDrink();
        
        DrinkData customeDrink = new DrinkData(drink.name, drink.Ingredients);
        customeDrink.addIngredient(addOn);
        return customeDrink;
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

    public override void NextMove()
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

   
}
