using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "DrinkData", menuName = "DrinkData/Generic")]
public class DrinkData : ScriptableObject
{
    [SerializeField] public new string name;
    public List<IngredientNode> ingredients;

    public float price;

    public void AddIngredient(Ingredients ingredient, float target)
    {
        ingredients.Add(new IngredientNode(ingredient, target));
    }

    public void AddIngredient(IngredientNode @in)
    {
        if (ingredients == null) return;
        var foundIngredient = false;
        foreach (var ingredient in ingredients.Where(ingredient => ingredient.ingredient == @in.ingredient))
        {
            foundIngredient = true;
            ingredient.target += @in.target;
        }

        if (!foundIngredient)
        {
            ingredients.Add(@in);
        }
    }

    public DrinkData(string name)
    {
        ((Object)this).name = name;
        ingredients = new List<IngredientNode>();
    }

    public DrinkData(string name, List<IngredientNode> ingredients)
    {
        ((Object)this).name = name;
        this.ingredients = new List<IngredientNode>();
        foreach (var tempNode in ingredients.Select(@in => new IngredientNode(@in.ingredient, @in.target)))
        {
            this.ingredients.Add(tempNode);
        }
    }

    public static float Compare(DrinkData playerDrink, DrinkData desiredDrink)
    {
        float sum = 0;
        float numberOfBadIngredients;
        float numberOfDesiredIngredients = desiredDrink.ingredients.Count;
        try
        {
            numberOfBadIngredients = playerDrink.ingredients.Count;
        }
        catch
        {
            numberOfBadIngredients = 0;
        }

        foreach (var desiredIngredientNode in desiredDrink.ingredients)
        {
            var foundIngredient = false;
            if (playerDrink && playerDrink.ingredients != null)
            {
                foreach (var playerIngredientNode in playerDrink.ingredients.Where(playerIngredientNode =>
                             desiredIngredientNode.ingredient == playerIngredientNode.ingredient))
                {
                    foundIngredient = true;
                    var min = Mathf.Min(desiredIngredientNode.target, playerIngredientNode.target);
                    var max = Mathf.Max(desiredIngredientNode.target, playerIngredientNode.target);
                    sum += min / max;
                    break;
                }
            }

            if (!foundIngredient)
            {
                numberOfBadIngredients++;
            }
        }

        var average = (sum / numberOfDesiredIngredients) - (numberOfBadIngredients * 0.05f);
        Debug.Log("Drink Comparison => average:" + average + "... or sum: " + sum + " >>> currently using sum");
        return sum;
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
    [SerializeField, Range(0, 10)] public float target;
}