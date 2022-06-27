using System.Collections;
using UnityEngine;

public class MilkContainer : MonoBehaviour
{
    private int _count;
    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return new WaitForSeconds(0.04f);
        StartCoroutine(AddIngredients());
    }
    
    private IEnumerator AddIngredients()
    {
        _count = _count + 1;
        yield return new WaitForSeconds(0.04f);
        var iN = new IngredientNode(Ingredients.Milk, .1f);
        GetComponent<IngredientContainer>().AddToContainer(iN);
        if (_count < 500)
        {
            StartCoroutine(AddIngredients());
        }
    }
}
