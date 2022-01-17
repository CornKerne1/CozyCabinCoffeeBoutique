using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchData", menuName = "ResearchData/Generic")]
public class ResearchData : ScriptableObject
{
    public List<DrinkData> Drinks;
}
