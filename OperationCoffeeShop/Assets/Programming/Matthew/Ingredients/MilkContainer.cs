using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkContainer : MonoBehaviour
{
    private int count;
    // Start is called before the first frame update
    void Start()
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
        count = count + 1;
        yield return new WaitForSeconds(0.04f);
        var iN = new IngredientNode(Ingredients.Milk, .1f);
        GetComponent<IngredientContainer>().AddToContainer(iN);
        if (count < 500)
        {
            StartCoroutine(AddIngredients());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
