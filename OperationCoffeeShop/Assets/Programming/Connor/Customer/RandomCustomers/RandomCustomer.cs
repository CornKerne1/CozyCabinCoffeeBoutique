using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public class RandomCustomer : Customer
{
    [SerializeField] private RandomNameSet nameSet;
    private string _customerName;
    public RandomCustomerSet customerSet;


    public new async void Awake()
    {

        await SpawnRandomCustomer();

        _customerName = nameSet.names[Random.Range(0, nameSet.names.Count)];

        var favoriteDrink = GetRandomDrink();

        SetUpCustomerData(favoriteDrink);

        var customerDrink = ScriptableObject.CreateInstance<DrinkData>();

        SetUpAndModifyDrinkData(customerDrink, favoriteDrink);
        base.Awake();
    }

    private async Task SpawnRandomCustomer()
    {
        int index=await GetSpawningIndex();
        var customer = Instantiate(customerSet.customers[ index]);
        var scale = customer.transform.localScale;
        var position = customer.transform.localPosition;
        customer.transform.parent = gameObject.transform; // do not refactor
        customer.transform.localPosition = position;
        customer.transform.localScale = scale;
    }

    private async Task<int> GetSpawningIndex()
    {
        int index = (int)Random.Range(0f, customerSet.customers.Count);
        
        if (customerSet.customerIndexInScene.Count == 0)
        {
            customerSet.customerIndexInScene.Add(index);
        }
        else
        {
            while(index==customerSet.customerIndexInScene[^1])
            {
                index = (int)Random.Range(0f, customerSet.customers.Count);
                await Task.Yield();
            }
            customerSet.customerIndexInScene.Add(index);
        }
        return index;
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
        customerData.soundEngineEnvent = "Play_SFX_Text";
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

    private void OnDestroy()
    {
        
    }
}