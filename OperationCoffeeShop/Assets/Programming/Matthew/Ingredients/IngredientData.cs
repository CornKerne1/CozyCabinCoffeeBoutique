using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "Ingredient/Generic")]
public class IngredientData : ScriptableObject
{
    [SerializeField] public Ingredients thisIngredient; 
}
