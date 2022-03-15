using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInteraction : Interactable
{

    public MachineData mD;
    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        gameObject.GetComponent<Machine>().StartMachine();
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }
}
