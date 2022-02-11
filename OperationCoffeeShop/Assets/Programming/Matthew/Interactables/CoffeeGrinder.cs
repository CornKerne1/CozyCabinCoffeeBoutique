using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGrinder : Interactable
{

    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

    public override void OnInteract()
    {
        StartCoroutine(gameObject.GetComponent<Machine>().ActivateMachine(5));
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }
}
