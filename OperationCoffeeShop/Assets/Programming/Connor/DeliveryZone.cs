using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class DeliveryZone : MonoBehaviour
{


    [SerializeField] public static event EventHandler DrinkDelivery;

  

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IngredientContainer>(out IngredientContainer drinkContainor) && drinkContainor.inHand ==false)
        {
            DrinkDelivery?.Invoke(other.gameObject, EventArgs.Empty);
            Debug.Log("Drink detected");
        }
    }


}
