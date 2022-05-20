using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractable : Interactable
{

    public CustomerAI CAI;

    public CustomerData CD;

    public Conversation conversation;

    private GameObject prompt;

    private DialogDisplay DD;

    public Canvas canvas;

    private OrderThoughts orderBubble;
    private Canvas orderCanvas;


    private CustomerAnimations CA;
    private void Start()
    {
        DD = GameObject.Find("Dialog").GetComponent<DialogDisplay>();
        prompt = GameObject.Find("Canvas");
        canvas = prompt.GetComponent<Canvas>();
        canvas.enabled = false;
        this.gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        CA = gameObject.GetComponent<CustomerAnimations>();
        orderBubble = gameObject.GetComponentInChildren<OrderThoughts>();
        orderCanvas = gameObject.GetComponentInChildren<Canvas>();
        
    }

    public override void OnFocus()
    {
        canvas.enabled = true; //instanciates on screen prompt asking if you want to interact with them.

    }

    public override void OnInteract(PlayerInteraction pI)
    {
        //Debug.Log("boop the customer");
        //invokes the dialogue interaction thing
        //DialogDisplay
        DD.AdvanceConversation();
        CA.Talk();
        DisplayOrderBubble();
        if ((DD.finishedOrder && !CAI.hasOrdered) || DD.finishedOrder && CAI.hasOrdered && CAI.hasOrder)
        {
            StartCoroutine(MoveLine());
            DisplayOrderTicket();
        }
        else if ( DD.finishedOrder && CAI.hasOrdered && CAI.hasOrder)
        {
            StartCoroutine(MoveLine());
            RemoveOrderTicket();
            RemoveOrderBubble();
        }
    }

    private void DisplayOrderBubble()
    {
        orderCanvas.enabled = true;
    }
    private void RemoveOrderBubble()
    {
        orderCanvas.enabled = false;
    }

    private void DisplayOrderTicket()
    {

    }
    private void RemoveOrderTicket()
    {

    }

    IEnumerator MoveLine()
    {
        yield return new WaitForSeconds(2);
        CAI.customerLines[CAI.customerLines.Count - 1].moveLine();

    }

    public override void OnLoseFocus()
    {
        canvas.enabled = false;//Destroys on screen prompt 
    }

    public override void OnAltInteract(PlayerInteraction pI)
    {
    }

    public override void OnAltInteractCanceled()
    {
    }

    public DialogDisplay GetDD()
    {
        return this.DD;
    }
}
