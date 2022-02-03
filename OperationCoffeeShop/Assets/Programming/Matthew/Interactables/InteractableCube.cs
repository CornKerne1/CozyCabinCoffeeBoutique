using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCube : Interactable
{
    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

   

    public override void OnInteract()
    {
        Debug.Log("SOMETHING");
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }
}
