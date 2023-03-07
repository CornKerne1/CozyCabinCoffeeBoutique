using System;
using System.Collections;
using UnityEngine;

public class Fridge : MonoBehaviour
{

    public void AddIngredient(object sender, EventArgs e)
    {
        try
        {
            Tuple<Ingredients, int> tuple = (Tuple<Ingredients, int>)sender;

            switch (tuple.Item1)
            {
                case Ingredients.Milk:
                    StartCoroutine(AddMilk());
                    break;
            }
        }
        catch
        {
            // ignored
        }
    }

    IEnumerator AddMilk()
    {
        yield return new WaitForSeconds(.4f);
    }
}