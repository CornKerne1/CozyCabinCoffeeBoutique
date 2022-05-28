using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SugarCube : MonoBehaviour
{
    public IngredientNode iN;
    [SerializeField] private GameObject sugarCube;
    

    private void OnTriggerEnter(Collider other)
    {
        TryAddOrDelete(other.gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(MakePickUpable());
    }

    public IEnumerator MakePickUpable()
    {
        yield return new WaitForSeconds(1.0f);
        Instantiate(sugarCube, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }
    
    private void TryAddOrDelete(GameObject obj)
    {
        try
        {
            obj.GetComponent<IngredientContainer>().AddToContainer(iN); //WRITE CODE THAT CHECKS IF THIS INGREDIENT IS ALREADY ON LIST. IF SO ONLY USE THE AMOUNT AND DONT ADD THE ARRAY ELEMENT;
            Destroy(gameObject);
        }
        catch
        {
            Destroy(gameObject);
        }
    }
}

