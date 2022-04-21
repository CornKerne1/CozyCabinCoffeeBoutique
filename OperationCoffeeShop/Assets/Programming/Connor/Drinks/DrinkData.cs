using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DrinkData", menuName = "DrinkData/Generic")]
public class DrinkData : ScriptableObject
{
    [SerializeField]
    public string Name;
    public List<IngredientNode> Ingredients;

    public float price;
    public void addIngredient(Ingredients ingredient, float target)
    {
        Ingredients.Add(new IngredientNode(ingredient, target));
    }

    /// <summary>
    /// if Drink has a particular Ingredent held in Ingredient Node:
    /// Add Target of parameter to value in list.
    /// Else: add Ingredient node to list. 
    /// </summary>
    /// <param name="IN"></param>
    public void addIngredient(IngredientNode IN)
    {
        if (Ingredients != null)
        {
            bool foundIngredient = false;
            foreach (IngredientNode ingredient in Ingredients)
            {
                if (ingredient.ingredient == IN.ingredient)
                {
                    foundIngredient = true;
                    ingredient.target += IN.target;
                }

            }
            if (!foundIngredient)
            {
                Ingredients.Add(IN);
            }
        }
    }

    public DrinkData(string name)
    {
        this.name = name;
        Ingredients = new List<IngredientNode>();

    }
    public DrinkData(string name, List<IngredientNode> Ingredients)
    {
        this.name = name;
        this.Ingredients = new List<IngredientNode>();
        foreach (IngredientNode IN in Ingredients)
        {
            IngredientNode tempNode = new IngredientNode(IN.ingredient, IN.target);
            this.Ingredients.Add(tempNode);
        }
    }

    /// <summary>
    /// Compares two drink datas to determine how approximate they are equivalent to eachother. 
    /// </summary>
    /// <param name="playerDrink"></param>
    /// <param name="DesiredDrink"></param>
    /// <returns>range between 0 and 1. 0 being nothing alike, 1 being aproximatily identical.</returns>
    public float Compare(DrinkData playerDrink, DrinkData DesiredDrink)
    {
        float sum = 0;
        float numberOfDesiredIngredients = DesiredDrink.Ingredients.Count;
        float numberofBadIngredients = playerDrink.Ingredients.Count;

        foreach (IngredientNode DesiredIngredientNode in DesiredDrink.Ingredients)
        {
            bool foundIngredient = false;
            foreach (IngredientNode playerIngredientNode in playerDrink.Ingredients)
            {
                if (DesiredIngredientNode.ingredient == playerIngredientNode.ingredient)
                {
                    foundIngredient = true;
                    float min = Mathf.Min(DesiredIngredientNode.target, playerIngredientNode.target);
                    float max = Mathf.Max(DesiredIngredientNode.target, playerIngredientNode.target);
                    sum += min / max;
                    break;
                }
            }
            if (!foundIngredient)
            {
                numberofBadIngredients++;
            }
        }
        float average = (sum / numberOfDesiredIngredients) - (numberofBadIngredients * 0.05f);//evaluation 
        return sum;
    }
}




[System.Serializable]
public class IngredientNode
{
    /// <summary>
    /// Constructs a new Ingredient node ready to be added to List;
    /// </summary>
    /// <param name="ingredient"></param>
    /// <param name="target">Range between 0 & 100</param>
    public IngredientNode(Ingredients ingredient, float target)
    {
        this.ingredient = ingredient;
        this.target = target;
    }
    public Ingredients ingredient;
    [SerializeField, Range(0, 100)]
    public float target;
}
