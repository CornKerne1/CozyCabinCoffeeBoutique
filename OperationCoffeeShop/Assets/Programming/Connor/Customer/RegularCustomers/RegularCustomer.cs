using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularCustomer : Customer
{
    private CustomerAI ai;
    [SerializeField, Header("If true ignore next variable")]
    public bool randomTimeOfDay;
    public int spawnTime;

    public void Awake()
    {
        customerData.customer = this;
        ai = GetComponent<CustomerAI>();
        customerData.orderedDrinkData = GetFavoriteDrink();
        //CD.DesiredFlavors(CD.favoriteDrinkData.);
    }
    

    public override DrinkData GetDrinkOrder()
    {
        return customerData.orderedDrinkData;
    }

    
}
