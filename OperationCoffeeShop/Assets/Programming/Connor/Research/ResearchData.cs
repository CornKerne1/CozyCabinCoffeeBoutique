using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ResearchData", menuName = "ResearchData/Generic")]
public class ResearchData : ScriptableObject
{
    [FormerlySerializedAs("Drinks")] public List<DrinkData> drinks;
}