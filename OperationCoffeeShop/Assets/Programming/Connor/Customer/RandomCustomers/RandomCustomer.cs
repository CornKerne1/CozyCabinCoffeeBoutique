using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomCustomer : Customer
{
    [SerializeField]
    private RandomNameSet nameSet;

    private string customerName;

    private CustomerAI ai;

    public RandomCustomerSet CustomerSet;


    public void Awake()
    {
        //Instanciate Random Customer from CustomerSet
        GameObject customer = Instantiate(CustomerSet.Customers[UnityEngine.Random.Range(0, CustomerSet.Customers.Count)]) as GameObject;
        Vector3 scale = customer.transform.localScale;
        Vector3 position = customer.transform.localPosition;
        customer.transform.parent = gameObject.transform;
        customer.transform.localPosition = position;
        customer.transform.localScale = scale;

        ai = GetComponent<CustomerAI>();

        //sets a random name from nameSet
        customerName = nameSet.names[UnityEngine.Random.Range(0, nameSet.names.Count)];
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
                foreach(FlavorProfile.FlavorNode f in flavorNodes)
                {
                    if( f.flavor == flavorProfile.flavorProfile[i.ingredient].flavor)
                    {
                        f.strength += flavorProfile.flavorProfile[i.ingredient].strength;
                    }
                }
            }
        }
        CD = (CustomerData)ScriptableObject.CreateInstance("CustomerData");
        CD.name = customerName;
        CD.favoriteDrink = favoriteDrink;
        CD.DesiredFlavors(flavorNodes);
        CD.customer = this;
       
        DrinkData drink = GetRandomDrink();

        DrinkData customeDrink = new DrinkData(drink.name, drink.Ingredients);

        customeDrink.price = favoriteDrink.price;// need to add random ingredients to price
        //Adds a little RNG to the drink orders
        //IngredientNode addOn = GetRandomAddOn();
        //customeDrink.addIngredient(addOn);
        CD.orderedDrink= customeDrink;
    }

    public RandomCustomer(RandomNameSet nameSet)
    {
        //Uses specified nameset to generate random name
        this.nameSet = nameSet;
        //sets a random name from nameSet
        customerName = nameSet.names[UnityEngine.Random.Range(0, nameSet.names.Count)];
    }

    protected override IngredientNode GetRandomAddOn()
    {
        int ingredient = UnityEngine.Random.Range(0, PRD.learnedIngredients.Count);
        float target = UnityEngine.Random.value;
        return new IngredientNode(PRD.learnedIngredients[ingredient], target);
    }

    protected override DrinkData GetRandomDrink()
    {
        return PRD.learnedDrinks[UnityEngine.Random.Range(0, PRD.learnedDrinks.Count)];
    }
    public override DrinkData GetDrinkOrder()
    {
       
        return CD.orderedDrink;
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
