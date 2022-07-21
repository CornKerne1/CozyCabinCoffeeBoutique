using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SugarCube : LiquidIngredients
{
    [SerializeField] private GameObject sugarCube;
    private IEnumerator _coRef;
    

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
        Instantiate(sugarCube, currentTrans.position, currentTrans.rotation);
        Destroy(transform.gameObject);
        _coRef = null;
    }
}