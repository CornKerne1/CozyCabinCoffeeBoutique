using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "CustomerData/Generic")]
public class CustomerData : ScriptableObject
{
    public string name;
    public DrinkData favoriteDrink;
    public List<DrinkData> acceptableDrinks;
    //public List<string> orderPhrases;


    public CustomerData()
    {
        
       
    }
}
