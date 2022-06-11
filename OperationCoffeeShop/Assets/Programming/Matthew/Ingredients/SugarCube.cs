using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SugarCube : MonoBehaviour
{
    [SerializeField] private IngredientNode iN;
    [SerializeField] private GameObject sugarCube;
    private IEnumerator _coRef;

    private void OnTriggerEnter(Collider other)
    {
        TryAddOrDelete(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_coRef != null) return;
        _coRef = CO_MakePickUp();
        StartCoroutine(CO_MakePickUp());
    }

    private IEnumerator CO_MakePickUp()
    {
        yield return new WaitForSeconds(0.2f);
        var currentTrans = transform;
        Instantiate(sugarCube, currentTrans.position, currentTrans.rotation).GetComponent<Rigidbody>().isKinematic =
            false;

        Destroy(transform.gameObject);
        _coRef = null;
    }

    private void TryAddOrDelete(GameObject obj)
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