using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerResearchData", menuName = "PlayerResearchData/Generic")]
public class PlayerResearchData : ScriptableObject
{
    [FormerlySerializedAs("RD")] public ResearchData researchData;

    [SerializeField] public List<Ingredients> learnedIngredients;

    [SerializeField] public List<DrinkData> learnedDrinks;
}