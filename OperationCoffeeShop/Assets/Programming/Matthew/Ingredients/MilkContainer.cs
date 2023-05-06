using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class MilkContainer : MonoBehaviour
{
    private int _count;
    private async void Start()
    {
        await Init();
    }

    private async Task Init()
    {
        await Task.Delay(40);
        await AddIngredients();
    }
    
    private async Task AddIngredients()
    {
        _count = _count + 1;
        await Task.Delay(15);
        var iN = new IngredientNode(Ingredients.Milk, .01f);
        GetComponent<IngredientContainer>().AddToContainer(iN);
        if (_count < 400)
        {
            await (AddIngredients());
        }
    }
}
