using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractable : Interactable
{

    public CustomerAI AI;

    public Conversation conversation;

    public GameObject prompt;

    private DialogDisplay DD;

    private void Start()
    {
        DD = GameObject.Find("Dialog").GetComponent<DialogDisplay>();
        prompt = GameObject.Find("Canvas");
        prompt.GetComponent<Canvas>().enabled = false;
    }

    public override void OnFocus()
    {
        prompt.GetComponent<Canvas>().enabled = true; //instanciates on screen prompt asking if you want to interact with them.
        
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        Debug.Log("boop the customer");
        //invokes the dialogue interaction thing
        //DialogDisplay
        DD.AdvanceConversation();

    }

    public override void OnLoseFocus()
    {

        prompt.GetComponent<Canvas>().enabled = false;//Destroys on screen prompt 
    }



}
