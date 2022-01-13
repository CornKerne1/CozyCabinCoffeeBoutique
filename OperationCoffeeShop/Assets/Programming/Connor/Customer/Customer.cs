using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Customer : MonoBehaviour
{


    /// <summary>
    ///      uses LearnedIngredients to randomly return any addons that a customer may want on top 
    ///      of the default ingredients for the random drink. 
    /// </summary>
    /// <returns></returns>
    abstract public List<Ingredients> GetRandomAddOns();


    /// <summary>
    ///      uses LearnedDrinks to randomly return a drink that this customer would want.

    /// </summary>
    /// <returns></returns>
    public abstract Drinks GetRandomDrink();


}
