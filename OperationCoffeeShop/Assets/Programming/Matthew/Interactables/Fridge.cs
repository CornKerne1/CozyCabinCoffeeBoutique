using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
{

    private void Start()
    {
        ComputerShop.DepositItems += AddIngredient;

    }
    public void AddIngredient(object sender, EventArgs e)
    {
        try
        {
            Tuple<Ingredients, int> tuple = (Tuple<Ingredients, int>)sender;

            switch (tuple.Item1){
                case Ingredients.Milk:
                    StartCoroutine(AddMilk());
                    break;

            }
        }
        catch
        {

        }
    }
    IEnumerator AddMilk()
    {
        yield return new WaitForSeconds(.4f);
    }
}