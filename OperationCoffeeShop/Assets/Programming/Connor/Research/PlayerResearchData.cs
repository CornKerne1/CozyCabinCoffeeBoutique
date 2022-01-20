using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerResearchData", menuName = "PlayerResearchData/Generic")]
public class PlayerResearchData : ScriptableObject
{
    public ResearchData RD;

    [SerializeField]
    public List<Ingredients> learnedIngredients;

    [SerializeField]
    public List<DrinkData> learnedDrinks;

}
