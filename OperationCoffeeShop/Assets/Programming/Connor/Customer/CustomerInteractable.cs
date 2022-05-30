using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerInteractable : Interactable
{

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualcue;

    private TextAsset IntroConversation;
    private TextAsset ExitConversation;

    public CustomerAI CAI;

    public CustomerData CD;

    public Conversation conversation;

    private GameObject prompt;


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

    [HideInInspector]
    public RegularCustomerAtlas rCA;

    private void Start()
    {
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

        // will be null if random customer or not spawned by a regular spawner
        if (rCA != null)
            SetConversations();
        else
        {
            StartCoroutine(SetRandomConversations());
        }
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
            gM.pD.neckClamp = neckclamp * 4;

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
            gM.pD.neckClamp = neckclamp * 4;


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
    private void SetConversations()
    {
        if (rCA.dic.ContainsKey(gM.gMD.currentTime.Day))
        {
            foreach (RegularCustomerAtlas.customerConversations cc in rCA.dic[gM.gMD.currentTime.Day])
            {
                if (cc.customer.GetComponent<Customer>().CD = this.CD)
                {
                    this.IntroConversation = cc.IntroConversation;
                    this.ExitConversation = cc.ExitConversation;
                    break;
                }
            }
        }
    }
    private IEnumerator SetRandomConversations()
    {
        yield return new WaitForSeconds(2);
        Customer rc = this.gameObject.GetComponent<RandomCustomer>();
        try
        {
            IntroConversation = rc.randomConversations.introConversations[Random.Range(0, rc.randomConversations.introConversations.Count)];
            ExitConversation = rc.randomConversations.exitConversations[Random.Range(0, rc.randomConversations.exitConversations.Count)];

        }
        catch
        {
            Debug.Log("random conversation list is empty");
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
            talking = true;
            DialogueManager.GetInstance().EnterDialogueMode(IntroConversation);
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




    public void DeliverDrink()
    {
        dialogueManager.SetCurrentCustomer(this.gameObject);
        DialogueManager.GetInstance().EnterDialogueMode(ExitConversation);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pm.canMove = false;
        pcc.canMove = false;
        CA.Talk();
    }
}
