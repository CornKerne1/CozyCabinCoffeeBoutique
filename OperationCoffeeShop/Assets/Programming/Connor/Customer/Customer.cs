using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Customer : MonoBehaviour 
{
    public bool hasOrder = false;

    [Header("RandomCustomer will be empty")]
    public CustomerData CD; //holds all realavent variables

    [Header("Should not be empty")]
    public PlayerResearchData PRD; //holds all possible ingredients and drinks

    public GameMode gameMode;

    public virtual void Start()
    {
        gameMode =  GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }
    /// <summary>
    /// Returns Customer's Name.
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        if (CD != null || CD.name != null)
            return "Naome";
        return CD.name;

    }
    /// <summary>
    /// Attempts to change customer's name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void SetName(string name)
    {
        CD.name = name;
        
    } 

    public DrinkData GetFavoriteDrink()
    {
        if (CD != null || CD.name != null)
           Debug.Log("Null Favorite drink or CustomerData ");
        return CD.favoriteDrink;
    }

    //public List<DrinkData> GetAcceptableDrinks()
    //{
    //   List<DrinkData> AD = new List<DrinkData>();
    //    foreach(DrinkData drink in CD.desiredFlavors)
    //    {
    //        AD.Add(drink);  
    //    }
    //    return AD; 
    //}


    /// <summary>
    ///      uses LearnedToppings to randomly return any topping that a customer 
    ///      may want on top of the default toppings for the random drink. 
    /// </summary>
    /// <returns></returns>
    protected abstract List<Ingredients> GetRandomToppings();



    /// <summary>
    ///      uses LearnedIngredients to randomly return any addon that a customer
    ///      may want on top of the default ingredients for the random drink. 
    /// </summary>
    /// <returns></returns>
    protected abstract IngredientNode GetRandomAddOn();


    /// <summary>
    ///      uses LearnedDrinks to randomly return a drink that this customer would want.

    /// </summary>
    /// <returns></returns>
    protected abstract DrinkData GetRandomDrink();

    /// <summary>
    /// uses GetRandomAddOn and GetRandomDrink to return
    /// a drink the customer may desired
    /// </summary>
    /// <returns></returns>
    public abstract DrinkData GetDrinkOrder();


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


    /// <summary>
    /// Some how given a drink and sends the customers requested drink and the paramater drink to the GameMode.
    /// </summary>
    public abstract void RecieveDrink(DrinkData Drink);
}
