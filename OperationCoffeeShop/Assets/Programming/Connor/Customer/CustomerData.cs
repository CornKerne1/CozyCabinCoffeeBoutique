using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "CustomerData/Generic")]
public class CustomerData : ScriptableObject
{
    public string name;
    public DrinkData favoriteDrink;
    public DrinkData orderedDrink;
    public DrinkData recievedDrink;
    public List<FlavorProfile.FlavorNode> desiredFlavors;
    public Customer customer;
    public List<TextAsset> OrderConversation;
    public Sprite portraitNeutral;
    public Sprite portraitHappy;
    public Sprite protraitAnoyed;
    public Sprite portraitAmazed;
    public Sprite buttonImage;
    public Sprite dialogueBoxImage;
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
    //public void DesiredFlavors(List<FlavorProfile.FlavorNode> flavors)
    //{
    //    desiredFlavors = new List<FlavorProfile.FlavorNode>();
    //    foreach (FlavorProfile.FlavorNode f in flavors)
    //    {
    //        this.desiredFlavors.Add(f);
    //    }
    //}

    //public class KeyValuePair
    //{
    //    public int Day;
    //    public List<GameObject> customers;
    //}

    //public List<KeyValuePair> customerDays = new List<KeyValuePair>();
    //public Dictionary<int, List<GameObject>> dic = new Dictionary<int, List<GameObject>>();

    //public void updateDictionary()
    //{
    //    foreach (var kvp in customerDays)
    //    {
    //        dic[kvp.Day] = kvp.customers;
    //    }
    //}
}
