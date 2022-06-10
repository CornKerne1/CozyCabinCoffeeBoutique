using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SugarCube : MonoBehaviour
{
    [SerializeField]private IngredientNode iN;
    [SerializeField] private GameObject sugarCube;
    private IEnumerator CO_Ref;
    

    private void OnTriggerEnter(Collider other)
    {
        TryAddOrDelete(other.gameObject);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CO_Ref != null) return;
        CO_Ref = MakePickUpable();
        StartCoroutine(MakePickUpable());
    }

    public IEnumerator MakePickUpable()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(sugarCube, transform.position, transform.rotation);
        Destroy(transform.gameObject);
        CO_Ref = null;
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

