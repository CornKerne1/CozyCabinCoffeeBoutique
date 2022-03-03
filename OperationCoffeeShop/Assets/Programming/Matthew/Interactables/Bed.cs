using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    public override void OnFocus()
    {
        Debug.Log("We Are Looking At You");
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        base.gM.gMD.sleepTime = base.gM.gMD.currentTime.AddHours(8);
        //base.gM.gMD.sleepTime.AddHours(8);
        base.gM.gMD.sleeping = true;
    }

    public override void OnLoseFocus()//
    {
        Debug.Log("Gone!");
    }
}
