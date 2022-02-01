using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DrinkData", menuName = "DrinkData/Generic")]
public class DrinkData : ScriptableObject
{
    [SerializeField]
    public string Name;
    public List<IngredientNode> Ingredients;
    
    public void addIngredient(Ingredients ingredient, float target)
    {
        Ingredients.Add(new IngredientNode(ingredient,target));
    }
    public void addIngredient(IngredientNode IN)
    {
        if (Ingredients != null)
        Ingredients.Add(new IngredientNode(IN.ingredient, IN.target));
    }
    public DrinkData(string name)
    {
        this.name = name;
        Ingredients = new List<IngredientNode>();

    }
    public DrinkData(string name, List<IngredientNode> Ingredients)
    {
        this.name=name;
        this.Ingredients = new List<IngredientNode>();
        foreach(IngredientNode IN in Ingredients){
            IngredientNode tempNode = new IngredientNode(IN.ingredient,IN.target);
            this.Ingredients.Add(tempNode);
        }
    }
}
[System.Serializable]
public class IngredientNode
    {
        public IngredientNode(Ingredients ingredient, float target)
        {
            this.ingredient = ingredient;
            this.target = target;
        }
        public Ingredients ingredient;
        [SerializeField, Range(0,1)]
        public float target;
    }
