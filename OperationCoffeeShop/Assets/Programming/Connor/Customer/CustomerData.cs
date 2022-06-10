using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "CustomerData/Generic")]
public class CustomerData : ScriptableObject
{
    
    public new string name;
    
    public DrinkData favoriteDrinkData;
    public DrinkData orderedDrinkData;
    public DrinkData receivedDrinkData;
    public List<FlavorProfile.FlavorNode> desiredFlavors;
    
    public Customer customer;
    [HideInInspector] public CustomerAnimations customerAnimations;
    [HideInInspector] public CustomerAI customerAI;
    [HideInInspector] public CustomerInteractable customerInteractable;


    public Sprite portraitNeutral;
    public Sprite portraitHappy;
    public Sprite portraitAnnoyed;
    public Sprite portraitAmazed;
    public Sprite buttonImage;
    public Sprite dialogueBoxImage;


    public CustomerData(string name, DrinkData favoriteDrinkData, List<FlavorProfile.FlavorNode> flavors)
    {
        this.name = name;
        this.favoriteDrinkData = favoriteDrinkData;


    }

    public void DesiredFlavors(List<FlavorProfile.FlavorNode> flavors)
    {
        desiredFlavors = new List<FlavorProfile.FlavorNode>();
        foreach (var f in flavors)
        {
            this.desiredFlavors.Add(f);
        }
    }
}
