using System.Collections.Generic;
using UnityEngine;


public class RandomCustomer : Customer
{
    [SerializeField] private RandomNameSet nameSet;

    private string _customerName;


    public RandomCustomerSet customerSet;


    public void Awake()
    {
        SpawnRandomCustomer();

        _customerName = nameSet.names[Random.Range(0, nameSet.names.Count)];

        var favoriteDrink = GetRandomDrink();

        SetUpCustomerData(favoriteDrink);

        var customerDrink = ScriptableObject.CreateInstance<DrinkData>();

        SetUpAndModifyDrinkData(customerDrink, favoriteDrink);
    }

    private void SpawnRandomCustomer()
    {
        var customer = Instantiate(customerSet.customers[Random.Range(0, customerSet.customers.Count)]);
        var scale = customer.transform.localScale;
        var position = customer.transform.localPosition;
        customer.transform.parent = gameObject.transform; // do not refactor
        customer.transform.localPosition = position;
        customer.transform.localScale = scale;
    }


    private void SetUpCustomerData(DrinkData favoriteDrink)
    {
        var flavors = new List<Flavors>();

        var flavorNodes = new List<FlavorProfile.FlavorNode>();

        var flavorProfile = new FlavorProfile();

        foreach (var
                     i in favoriteDrink.ingredients)
        {
            if (!flavors.Contains(flavorProfile.flavorProfile[i.ingredient].flavor))
            {
                flavors.Add(flavorProfile.flavorProfile[i.ingredient].flavor);
                flavorNodes.Add(flavorProfile.flavorProfile[i.ingredient]);
            }
            else
            {
                foreach (var f in flavorNodes)
                {
                    if (f.flavor == flavorProfile.flavorProfile[i.ingredient].flavor)
                    {
                        f.strength += flavorProfile.flavorProfile[i.ingredient].strength;
                    }
                }
            }
        }

        customerData = (CustomerData)ScriptableObject.CreateInstance("CustomerData");
        customerData.soundEngineEnvent = "PLAY_GENERICVOICE";
        customerData.name = _customerName;
        customerData.favoriteDrinkData = favoriteDrink;
        customerData.DesiredFlavors(flavorNodes);
        customerData.customer = this;
    }

    private void SetUpAndModifyDrinkData(DrinkData customerDrink, DrinkData favoriteDrink)
    {
        ((Object)customerDrink).name = ((Object)favoriteDrink).name;
        customerDrink.ingredients = favoriteDrink.ingredients;

        customerDrink.price = favoriteDrink.price; // need to add random ingredients to price
        //Adds a little RNG to the drink orders
        //IngredientNode addOn = GetRandomAddOn();
        //customerDrink.addIngredient(addOn);
        customerData.orderedDrinkData = customerDrink;
    }

    public RandomCustomer(RandomNameSet nameSet)
    {
        this.nameSet = nameSet;
        _customerName = nameSet.names[Random.Range(0, nameSet.names.Count)];
    }

    protected IngredientNode GetRandomAddOn()
    {
        var ingredient = Random.Range(0, playerResearchData.learnedIngredients.Count);
        var target = Random.value;
        return new IngredientNode(playerResearchData.learnedIngredients[ingredient], target);
    }

    private DrinkData GetRandomDrink()
    {
        return playerResearchData.learnedDrinks[Random.Range(0, playerResearchData.learnedDrinks.Count)];
    }

    public override DrinkData GetDrinkOrder()
    {
        return customerData.orderedDrinkData;
    }
}