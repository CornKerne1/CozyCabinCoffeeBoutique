using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractable : Interactable
{

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualcue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    public CustomerAI CAI;

    public CustomerData CD;

    public Conversation conversation;

    private GameObject prompt;

    private CustomerDialogue customerDialogue;

    public Canvas canvas;

    private OrderThoughts orderBubble;
    private Canvas orderCanvas;

    private GameObject player;
    private PlayerMovement pm;
    private PlayerCameraController pcc;

    private CustomerAnimations CA;

    private CustomerLine line;

    float neckclamp;

    Transform oldLook;

    public bool talking = false;

    DialogueManager dialogueManager;

    private void Start()
    {
        customerDialogue = GameObject.Find("Dialogue Canvas").GetComponent<CustomerDialogue>();
        this.gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        CA = gameObject.GetComponent<CustomerAnimations>();
        orderBubble = gameObject.GetComponentInChildren<OrderThoughts>();
        orderCanvas = gameObject.GetComponentInChildren<Canvas>();
        orderCanvas.enabled = false;
        player = gM.player.gameObject;
        pm = player.GetComponent<PlayerMovement>();
        pcc = player.GetComponent<PlayerCameraController>();
        neckclamp = gM.pD.neckClamp;
        dialogueManager = DialogueManager.GetInstance();
    }
    private void Update()
    {
        if (dialogueManager.finishedConversation && CAI.hasOrder && CAI.hasOrdered)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Camera.main.transform.LookAt(oldLook.position);
            pm.canMove = true;
            pcc.canMove = true;
            StartCoroutine(MoveLine());
            RemoveOrderBubble();
            RemoveOrderTicket();
            dialogueManager.finishedConversation = false;
            gM.pD.neckClamp = neckclamp;

        }
        else if (dialogueManager.finishedConversation && !CAI.hasOrdered)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Camera.main.transform.LookAt(oldLook.position);
            pm.canMove = true;
            pcc.canMove = true;
            StartCoroutine(MoveLine());
            DisplayOrderBubble();
            DisplayOrderTicket();
            dialogueManager.finishedConversation = false;
            gM.pD.neckClamp = neckclamp;


        }
        if (!pm.canMove && dialogueManager.GetCurrentCustomer() == this.gameObject)
        {
            gM.pD.neckClamp = neckclamp / 4;
            var c = Camera.main.transform;
            oldLook = c;
            Camera.main.transform.LookAt(this.transform.position);
            //gM.player.transform.LookAt(this.transform.position);
        }
    }
    public override void OnFocus()
    {
        visualcue.SetActive(true);
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        //invokes the dialogue interaction thing
        //DialogDisplay
        if (!dialogueManager.dialogueIsPlaying && CAI.stay == true && !CAI.hasOrdered)
        {
            dialogueManager.SetCurrentCustomer(this.gameObject);
            customerDialogue.startedConversation = true;
            talking = true;
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            //customerDialogue.ChangeConversation(customerDialogue.converstation.conversationTreeOrder,this.transform);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //customerDialogue.canvas.enabled = true;
            pm.canMove = false;
            pcc.canMove = false;
            CA.Talk();
        }


    }

    public void DisplayOrderBubble()
    {
        orderCanvas.enabled = true;
    }
    public void RemoveOrderBubble()
    {
        orderCanvas.enabled = false;
    }

    public void DisplayOrderTicket()
    {

    }
    public void RemoveOrderTicket()
    {

    }

    IEnumerator MoveLine()
    {
        yield return new WaitForSeconds(2);
        CAI.customerLines[CAI.customerLines.Count - 1].moveLine();

    }

    public override void OnLoseFocus()
    {
        visualcue.SetActive(false);
    }

    public override void OnAltInteract(PlayerInteraction pI)
    {
    }
    

    public CustomerDialogue GetDD()
    {
        return this.customerDialogue;
    }

    public void DeliverDrink()
    {
        customerDialogue.ChangeConversation(customerDialogue.converstation.conversationTreeRecievedDrink,this.transform);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        customerDialogue.canvas.enabled = true;
        pm.canMove = false;
        pcc.canMove = false;
        CA.Talk();
    }
}
