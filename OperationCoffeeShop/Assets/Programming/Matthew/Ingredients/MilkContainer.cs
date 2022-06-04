using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MilkContainer : MonoBehaviour
{
    private int count;
    IngredientContainer iC;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
        ComputerShop.DepositItems += AddItems;
        iC = GetComponent<IngredientContainer>();
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(0.04f);
        StartCoroutine(AddIngredients(500));
    }

    private IEnumerator AddIngredients(int quantity)
    {
        count = count + 1;
        yield return new WaitForSeconds(0.04f);
        var iN = new IngredientNode(Ingredients.Milk, .1f);
        iC.AddToContainer(iN);
        if (count < quantity)
        {
            StartCoroutine(AddIngredients(quantity));
        }
    }
    public void AddItems(object sender, EventArgs e)
    {
        try
        {
            Tuple<Ingredients, int> tuple = (Tuple<Ingredients, int>)sender;

            if (Ingredients.Milk == tuple.Item1)
            {
                this.count += tuple.Item2;
                StartCoroutine(AddIngredients(count + tuple.Item2*10));
            }
        }
        catch
        {

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
