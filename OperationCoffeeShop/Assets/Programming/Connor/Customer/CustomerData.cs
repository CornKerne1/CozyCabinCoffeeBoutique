using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "CustomerData/Generic")]
public class CustomerData : ScriptableObject
{
    public string name;
    public DrinkData favoriteDrink;
    public List<FlavorProfile.FlavorNode> desiredFlavors;
    //public List<string> orderPhrases;


    public CustomerData(string name, DrinkData favoriteDrink, List<FlavorProfile.FlavorNode> flavors)
    {
        desiredFlavors = new List<FlavorProfile.FlavorNode>();
        this.name = name;
        this.favoriteDrink = favoriteDrink;
        foreach (FlavorProfile.FlavorNode f in flavors)
        {
            this.desiredFlavors.Add(f);
        }

    }
    public CustomerData()
    {

    }
}
