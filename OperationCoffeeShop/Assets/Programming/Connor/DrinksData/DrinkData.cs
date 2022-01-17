using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrinkData", menuName = "DrinkData/Generic")]
public class DrinkData : ScriptableObject
{
    [SerializeField]
    public string Name;
    public List<Nodes> Ingredients;
    [System.Serializable]
    public class Nodes
    {
        public Ingredients Ingredient;
        public float target;
    }

}
