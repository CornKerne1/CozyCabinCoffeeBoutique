using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularCustomer : Customer
{
    private CustomerAI _customerAI;

    [SerializeField, Header("If true ignore next variable")]
    public bool randomTimeOfDay;

    public int spawnTime;

    public new void Awake()
    {
        customerData.customer = this;
        _customerAI = GetComponent<CustomerAI>();
        StartCoroutine(CO_DrinkData());
        //CD.DesiredFlavors(CD.favoriteDrinkData.);
        base.Awake();
    }

    IEnumerator CO_DrinkData()
    {
        yield return new WaitForSeconds(0.4f);
        customerData.orderedDrinkData = GetFavoriteDrink();
    }

    public override DrinkData GetDrinkOrder()
    {
        return customerData.orderedDrinkData;
    }
}