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
        try
        {
            _count = _count + 1;
            await Task.Delay(5);
            var iN = new IngredientNode(Ingredients.Milk, .01f);
            GetComponent<IngredientContainer>().AddToContainer(iN);
            if (_count < 400)
            {
                await (AddIngredients());
            }
        }
        catch
        {
            Debug.LogWarning("Milk Container not filled completely before exit");
        }
    }
}
