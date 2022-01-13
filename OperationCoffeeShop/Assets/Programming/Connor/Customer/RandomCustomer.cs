using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomCustomer : Customer
{


    public override List<Ingredients> GetRandomAddOns()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// uses ValidDrinks to randomly return a drink that this customer 
    /// would want.
    /// </summary>
    /// <returns></returns>
    public override Drinks GetRandomDrink()
    {
        throw new NotImplementedException();
    }
}
