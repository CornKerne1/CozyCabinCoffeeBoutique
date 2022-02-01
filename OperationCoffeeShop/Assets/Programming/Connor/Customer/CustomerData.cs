using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "CustomerData/Generic")]
public class CustomerData : ScriptableObject
{
    public string name;
    public DrinkData favoriteDrink;
    public List<Flavors> desiredFlavors;
    //public List<string> orderPhrases;


    public CustomerData(string name, DrinkData favoriteDrink, List<Flavors> flavors)
    {
        desiredFlavors = new List<Flavors>();
        this.name = name;
        this.favoriteDrink = favoriteDrink;
        foreach (Flavors f in flavors)
        {
            this.desiredFlavors.Add(f);
        }

    }
    public CustomerData()
    {

    }
}
