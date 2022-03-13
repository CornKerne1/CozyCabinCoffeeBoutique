using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Ingredients
{
    Milk, SteamedMilk,FoamedMilk, Sugar, WhippedCream,
    EspressoShot, UngroundCoffee, GroundCoffee, Salt, BrewedCoffee

}
public enum Flavors
{
    Bitter,Sweet, Salty
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

        flavorProfile.Add(Ingredients.EspressoShot, new FlavorNode(Flavors.Bitter, Strength.Strong));
        flavorProfile.Add(Ingredients.GroundCoffee, new FlavorNode(Flavors.Bitter, Strength.Medium));
        flavorProfile.Add(Ingredients.UngroundCoffee, new FlavorNode(Flavors.Bitter, Strength.Medium));

        flavorProfile.Add(Ingredients.Salt, new FlavorNode(Flavors.Salty, Strength.Medium));

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
