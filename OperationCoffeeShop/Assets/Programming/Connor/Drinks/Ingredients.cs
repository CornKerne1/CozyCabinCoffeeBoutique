using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum  Ingredients 
{
    Milk, Sugar,
    EspressoShot, CofeeBean, GroundCoffeeBean,
    Salt

}
public enum Flavors
{
    Bitter,Sweet,Salty
}

public class FlavorProfile
{
    public Dictionary<Ingredients, Flavors> flavorProfile;

     
    public FlavorProfile()
    {
        flavorProfile = new Dictionary<Ingredients, Flavors>();
        flavorProfile.Add(Ingredients.Milk, Flavors.Sweet);
        flavorProfile.Add(Ingredients.Sugar, Flavors.Sweet);
        flavorProfile.Add(Ingredients.EspressoShot, Flavors.Bitter);
        flavorProfile.Add(Ingredients.CofeeBean, Flavors.Bitter);
        flavorProfile.Add(Ingredients.GroundCoffeeBean, Flavors.Bitter);
        flavorProfile.Add(Ingredients.Salt, Flavors.Salty);

    }
}
