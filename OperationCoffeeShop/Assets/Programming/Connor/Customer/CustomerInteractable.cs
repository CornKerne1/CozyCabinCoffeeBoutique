using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractable : Interactable
{

    public CustomerAI AI;

    public Conversation conversation;

    public GameObject prompt;
    public override void OnFocus()
    {
       
        Instantiate(prompt); //instanciates on screen prompt asking if you want to interact with them.
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        //invokes the dialogue interaction thing
        //DialogDisplay

    }

    public override void OnLoseFocus()
    {
       
        Destroy(prompt); //Destroys on screen prompt 
    }


    
}
