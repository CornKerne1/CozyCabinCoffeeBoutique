using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomCustomer : Customer
{
    [SerializeField] private RandomNameSet nameSet;

    private string _customerName;

    private CustomerAI _customerAI;

    public RandomCustomerSet customerSet;


    public void Awake()
    {
        //Instanciate Random Customer from CustomerSet
        var customer = Instantiate(customerSet.Customers[UnityEngine.Random.Range(0, customerSet.Customers.Count)],
            gameObject.transform, true) as GameObject;
        var scale = customer.transform.localScale;
        var position = customer.transform.localPosition;
        customer.transform.localPosition = position;
        customer.transform.localScale = scale;

        _customerAI = GetComponent<CustomerAI>();

        //sets a random name from nameSet
        _customerName = nameSet.names[UnityEngine.Random.Range(0, nameSet.names.Count)];
        DrinkData favoriteDrink = GetRandomDrink();

        List<Flavors> flavors = new List<Flavors>();

        List<FlavorProfile.FlavorNode> flavorNodes = new List<FlavorProfile.FlavorNode>();

        FlavorProfile flavorProfile = new FlavorProfile();

        foreach (IngredientNode i in favoriteDrink.Ingredients)
        {
            if (!flavors.Contains(flavorProfile.flavorProfile[i.ingredient].flavor))
            {
                flavors.Add(flavorProfile.flavorProfile[i.ingredient].flavor);
                flavorNodes.Add(flavorProfile.flavorProfile[i.ingredient]);
            }
            else
            {
                foreach (FlavorProfile.FlavorNode f in flavorNodes)
                {
                    if (f.flavor == flavorProfile.flavorProfile[i.ingredient].flavor)
                    {
                        f.strength += flavorProfile.flavorProfile[i.ingredient].strength;
                    }
                }
            }
        }

        customerData = (CustomerData)ScriptableObject.CreateInstance("CustomerData");
        customerData.name = _customerName;
        customerData.favoriteDrinkData = favoriteDrink;
        customerData.DesiredFlavors(flavorNodes);
        customerData.customer = this;

        var drink = GetRandomDrink();

        var customerDrink = new DrinkData(drink.name, drink.Ingredients);

        customerDrink.price = favoriteDrink.price; // need to add random ingredients to price
        //Adds a little RNG to the drink orders
        //IngredientNode addOn = GetRandomAddOn();
        //customerDrink.addIngredient(addOn);
        customerData.orderedDrinkData = customerDrink;
    }

    public RandomCustomer(RandomNameSet nameSet)
    {
        //Uses specified nameset to generate random name
        this.nameSet = nameSet;
        //sets a random name from nameSet
        _customerName = nameSet.names[UnityEngine.Random.Range(0, nameSet.names.Count)];
    }

    protected IngredientNode GetRandomAddOn()
    {
        var ingredient = UnityEngine.Random.Range(0, playerResearchData.learnedIngredients.Count);
        var target = UnityEngine.Random.value;
        return new IngredientNode(playerResearchData.learnedIngredients[ingredient], target);
    }

    private DrinkData GetRandomDrink()
    {
        return playerResearchData.learnedDrinks[UnityEngine.Random.Range(0, playerResearchData.learnedDrinks.Count)];
    }

    public override DrinkData GetDrinkOrder()
    {
        return customerData.orderedDrinkData;
    }
}