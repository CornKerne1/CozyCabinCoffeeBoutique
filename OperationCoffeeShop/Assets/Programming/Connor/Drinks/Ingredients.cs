using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//If you intend to add a new Ingredient to the enum,
//you must also add a new FlavorProfile for that Ingredient
public enum Ingredients 
{
    Milk, SteamedMilk,FoamedMilk, Sugar, WhippedCream,
    Espresso, UngroundCoffee, GroundCoffee, Salt, BrewedCoffee, EspressoBeans, CoffeeFilter, TeaBag

}
public enum Flavors
{
    Bitter, Sweet, Salty, Herby
}

public enum Strength
{
    Light, Medium, Strong
}

public class FlavorProfile
{
    public Dictionary<Ingredients, FlavorNode> flavorProfile;


    public FlavorProfile()
    {
        flavorProfile = new Dictionary<Ingredients, FlavorNode>();
        flavorProfile.Add(Ingredients.Milk, new FlavorNode(Flavors.Sweet, Strength.Light));
        flavorProfile.Add(Ingredients.SteamedMilk, new FlavorNode(Flavors.Sweet, Strength.Light));
        flavorProfile.Add(Ingredients.FoamedMilk, new FlavorNode(Flavors.Sweet, Strength.Light));
        flavorProfile.Add(Ingredients.Sugar, new FlavorNode(Flavors.Sweet, Strength.Medium));
        flavorProfile.Add(Ingredients.WhippedCream, new FlavorNode(Flavors.Sweet, Strength.Light));

        flavorProfile.Add(Ingredients.Espresso, new FlavorNode(Flavors.Bitter, Strength.Strong));
        flavorProfile.Add(Ingredients.GroundCoffee, new FlavorNode(Flavors.Bitter, Strength.Medium));
        flavorProfile.Add(Ingredients.UngroundCoffee, new FlavorNode(Flavors.Bitter, Strength.Medium));
        flavorProfile.Add(Ingredients.BrewedCoffee, new FlavorNode(Flavors.Bitter, Strength.Medium));

        flavorProfile.Add(Ingredients.Salt, new FlavorNode(Flavors.Salty, Strength.Medium));

        flavorProfile.Add(Ingredients.TeaBag, new FlavorNode(Flavors.Herby, Strength.Strong));

    }
    [System.Serializable]
    public class FlavorNode
    {
        public FlavorNode(Flavors flavor, Strength strength)
        {
            this.flavor = flavor;
            this.strength = (int)strength + 1;
        }
        public Flavors flavor;
        [SerializeField]
        public int strength;
    }
}
