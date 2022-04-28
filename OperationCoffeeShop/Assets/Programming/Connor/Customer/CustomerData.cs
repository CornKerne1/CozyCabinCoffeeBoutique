using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "CustomerData/Generic")]
public class CustomerData : ScriptableObject
{
    public string name;
    public DrinkData favoriteDrink;
    public DrinkData recievedDrink;
    public List<FlavorProfile.FlavorNode> desiredFlavors;
    public Customer customer;
    //public List<string> orderPhrases;


    public CustomerData(string name, DrinkData favoriteDrink, List<FlavorProfile.FlavorNode> flavors)
    {
        this.name = name;
        this.favoriteDrink = favoriteDrink;


    }
    public void DesiredFlavors(List<FlavorProfile.FlavorNode> flavors)
    {
        desiredFlavors = new List<FlavorProfile.FlavorNode>();
        foreach (FlavorProfile.FlavorNode f in flavors)
        {
            this.desiredFlavors.Add(f);
        }
    }
}
