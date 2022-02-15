using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGrinder : Interactable
{

    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        gameObject.GetComponent<Machine>().StartMachine(2);
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }
}
