using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LiquidIngredients : MonoBehaviour
{
    [SerializeField] private IngredientNode iN;

    protected virtual void OnTriggerEnter(Collider other)
    {
        TryAddOrDelete(other.gameObject);
    }

    protected virtual void TryAddOrDelete(GameObject obj)
    {
        try
        {
            obj.GetComponent<IngredientContainer>()
                .AddToContainer(
                    iN); //WRITE CODE THAT CHECKS IF THIS INGREDIENT IS ALREADY ON LIST. IF SO ONLY USE THE AMOUNT AND DONT ADD THE ARRAY ELEMENT;
            Destroy(gameObject);
        }
        catch
        {
            Destroy(gameObject);
        }
    }
}
