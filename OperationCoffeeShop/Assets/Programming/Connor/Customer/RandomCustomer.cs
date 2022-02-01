using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomCustomer : Customer
{
    [SerializeField]
    private RandomNameSet nameSet;

    private string customerName;

    public void Awake()
    {

        //sets a random name from nameSet
        customerName = nameSet.names[Random.Range(0, nameSet.names.Count)];
        DrinkData favoriteDrink = GetRandomDrink();
        List<Flavors> flavors = new List<Flavors>();
        FlavorProfile flavorProfile = new FlavorProfile();
        foreach (IngredientNode i in favoriteDrink.Ingredients)
        {
            if (!flavors.Contains(flavorProfile.flavorProfile[i.ingredient]))
            {
                flavors.Add(flavorProfile.flavorProfile[i.ingredient]);
            }
        }
        CD = new CustomerData(customerName, favoriteDrink, flavors);
    }

    public RandomCustomer(RandomNameSet nameSet)
    {
        //Uses specified nameset to generate random name
        this.nameSet = nameSet;
        //sets a random name from nameSet
        customerName = nameSet.names[Random.Range(0, nameSet.names.Count)];
    }

    protected override IngredientNode GetRandomAddOn()
    {
        int ingredient = Random.Range(0, PRD.learnedIngredients.Count);
        float target = Random.value;
        return new IngredientNode(PRD.learnedIngredients[ingredient], target);
    }

    protected override DrinkData GetRandomDrink()
    {
        return PRD.learnedDrinks[Random.Range(0, PRD.learnedDrinks.Count)];
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
        return new List<Ingredients>();
    }
    public override void NextMove()
    {

    }

    protected override Tree DialogueTree()
    {
        return new Tree();
    }

    public override string Dialogue()
    {
        return "";
    }


}
