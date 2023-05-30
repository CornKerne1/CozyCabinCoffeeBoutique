using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class MilkContainer : IngredientContainer
{
    public override async void Start()
    {
        base.Start();
        AddIngredients();
    }
    private void AddIngredients()
    {
        var iN = new IngredientNode(Ingredients.Milk, 800);
        AddToContainer(iN,Color.white);
        for (int i = 0; i < 799; i++)
        {
            outputIngredients.Add(iD.milk);
        }
    }
}
