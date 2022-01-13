using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Generic")]
public class PlayerData : ScriptableObject
{
    public float mouseSensitivity = 1;

    public List<Ingredients.Ingredient> LearnedIngredients;

    public List<Drinks.Drink> LearnedDrinks;

}
