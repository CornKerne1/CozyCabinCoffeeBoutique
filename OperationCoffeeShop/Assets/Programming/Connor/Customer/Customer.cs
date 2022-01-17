using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Customer : MonoBehaviour
{
    public PlayerResearchData PRD;

    /// <summary>
    /// Returns Customer's Name.
    /// </summary>
    /// <returns></returns>
    public abstract string Name();



    /// <summary>
    ///      uses LearnedToppings to randomly return any topping that a customer 
    ///      may want on top of the default toppings for the random drink. 
    /// </summary>
    /// <returns></returns>
    protected abstract List<Ingredients> GetRandomToppings();



    /// <summary>
    ///      uses LearnedIngredients to randomly return any addons that a customer
    ///      may want on top of the default ingredients for the random drink. 
    /// </summary>
    /// <returns></returns>
    protected abstract List<Ingredients> GetRandomAddOns();


    /// <summary>
    ///      uses LearnedDrinks to randomly return a drink that this customer would want.

    /// </summary>
    /// <returns></returns>
    protected abstract Drinks GetRandomDrink();



    /// <summary>
    /// Should return a Tree with the head containing children that represent the next
    /// possible dialogue options for the player
    /// </summary>
    /// <returns></returns>
    protected abstract Tree DialogueTree();


    /// <summary>
    /// Returns the next dialogue to be expressed to the player
    /// </summary>
    /// <returns></returns>
    public abstract string Dialogue();

    /// <summary>
    /// lerps customer to next logical location
    /// aka moving them to new location based on enviromental factors.
    /// </summary>
    public abstract void NextMove();
}
