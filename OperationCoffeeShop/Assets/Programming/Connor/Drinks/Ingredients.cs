using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

//If you intend to add a new Ingredient to the enum,
//you must also add a new FlavorProfile for that Ingredient
public enum Ingredients 
{
    Milk, SteamedMilk,FoamedMilk, Sugar, WhippedCream,
    Espresso, UngroundCoffee, GroundCoffee, Salt, BrewedCoffee, EspressoBeans, CoffeeFilter, TeaBag, Water,Vanilla,Mocha

}
public enum Flavors
{
    Bitter, Sweet, Salty, Herby, Water
}

public enum Strength
{
    Light, Medium, Strong
}

public class FlavorProfile
{
    public readonly Dictionary<Ingredients, FlavorNode> flavorProfile;


    public FlavorProfile()
    {
        flavorProfile = new Dictionary<Ingredients, FlavorNode>
        {
            { Ingredients.Milk, new FlavorNode(Flavors.Sweet, Strength.Light) },
            { Ingredients.SteamedMilk, new FlavorNode(Flavors.Sweet, Strength.Light) },
            { Ingredients.FoamedMilk, new FlavorNode(Flavors.Sweet, Strength.Light) },
            { Ingredients.Sugar, new FlavorNode(Flavors.Sweet, Strength.Medium) },
            { Ingredients.WhippedCream, new FlavorNode(Flavors.Sweet, Strength.Light) },
            { Ingredients.Espresso, new FlavorNode(Flavors.Bitter, Strength.Strong) },
            { Ingredients.GroundCoffee, new FlavorNode(Flavors.Bitter, Strength.Medium) },
            { Ingredients.UngroundCoffee, new FlavorNode(Flavors.Bitter, Strength.Medium) },
            { Ingredients.BrewedCoffee, new FlavorNode(Flavors.Bitter, Strength.Medium) },
            { Ingredients.Salt, new FlavorNode(Flavors.Salty, Strength.Medium) },
            { Ingredients.TeaBag, new FlavorNode(Flavors.Herby, Strength.Strong) },
            {Ingredients.Water, new FlavorNode(Flavors.Water, Strength.Strong) },
            {Ingredients.Vanilla, new FlavorNode(Flavors.Sweet, Strength.Strong) },
            {Ingredients.Mocha, new FlavorNode(Flavors.Sweet, Strength.Strong) },
        };
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
