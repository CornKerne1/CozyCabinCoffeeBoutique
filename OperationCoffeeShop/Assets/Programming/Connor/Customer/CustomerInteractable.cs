using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomerInteractable : Interactable
{

    [Header("Visual Cue")]

    private TextAsset _introConversation;
    private TextAsset _exitConversation;

    [FormerlySerializedAs("CAI")] public CustomerAI customerAI;

    private GameObject _prompt;


    public Canvas canvas;

    private Canvas _orderCanvas;

    private GameObject _player;

    private CustomerData _customerData;

    private CustomerLine _line;

    private float _neckclamp;

    private Transform _oldLook;

    private bool _canInteract = true;

    public DialogueManager dialogueManager;
    private PlayerInteraction _playerInteraction;

    //[HideInInspector]
    [FormerlySerializedAs("rCA")] public RegularCustomerAtlas regularCustomerAtlas;

    [FormerlySerializedAs("lookat")] public Transform lookAt;
    private Camera _camera;

    public override void Start()
    {
        base.Start();
        _camera = Camera.main;
        gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        _customerData = gameObject.GetComponent<Customer>().customerData;
        StartCoroutine(CO_AddSelfToData());  
        _orderCanvas = gameObject.GetComponentInChildren<Canvas>();
        _orderCanvas.enabled = false;
        _player = gM.player.gameObject;
        _playerInteraction = _player.GetComponent<PlayerInteraction>();
        _neckclamp = gM.pD.neckClamp;
        dialogueManager = DialogueManager.GetInstance();

        // will be null if random customer or not spawned by a regular spawner
        if (regularCustomerAtlas != null)
            SetConversations();
        else
        {

            StartCoroutine(SetRandomConversations());
        }
    }
    private void Update()
    {
        switch (dialogueManager.finishedConversation)
        {
            case true when customerAI.hasOrder && customerAI.hasOrdered:
            {
                _canInteract = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if (_oldLook != null)
                    _camera.transform.LookAt(_oldLook.position);
                gM.pD.canMove = true;
                gM.pD.canMove = true;
                StartCoroutine(MoveLine());
                RemoveOrderBubble();
                RemoveOrderTicket();
                dialogueManager.finishedConversation = false;
                gM.pD.neckClamp = 77.3f;
                _playerInteraction.pD.inUI = false;
                break;
            }
            case true when dialogueManager.GetCurrentCustomer() == this.gameObject:
            {
                Debug.Log("testing my patience");

                _canInteract = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if (_oldLook != null)
                    _camera.transform.LookAt(_oldLook.position);
                gM.pD.canMove = true;
                gM.pD.canMove = true;
                //StartCoroutine(MoveLine());
                DisplayOrderBubble();
                DisplayOrderTicket();
                dialogueManager.finishedConversation = false;
                gM.pD.neckClamp = 77.3f;
                _playerInteraction.pD.inUI = false;
                break;
            }
        }

        if (gM.pD.canMove || dialogueManager.GetCurrentCustomer() != this.gameObject ||
            !dialogueManager.dialogueIsPlaying) return;
        gM.pD.neckClamp = 0;
        var c = _camera.transform;
        _oldLook = c;
        _camera.transform.LookAt(lookAt.position);
    }

    private IEnumerator CO_AddSelfToData()
    {
        yield return new WaitForSeconds(.2f);
        _customerData.customerInteractable = this;

    }
    private void SetConversations()
    {
        if (!regularCustomerAtlas.dic.ContainsKey(gM.gMD.currentTime.Day)) return;
        foreach (var cc in regularCustomerAtlas.dic[gM.gMD.currentTime.Day].Where(cc => cc.customer.GetComponent<Customer>().customerData == customerAI.customerData))
        {
            this._introConversation = cc.IntroConversation;
            this._exitConversation = cc.ExitConversation;
            break;
        }
    }
    private IEnumerator SetRandomConversations()
    {
        yield return new WaitForSeconds(2);
        Customer rc = gameObject.GetComponent<RandomCustomer>();
        try
        {
            _introConversation = rc.randomConversations.introConversations[Random.Range(0, rc.randomConversations.introConversations.Count)];
            _exitConversation = rc.randomConversations.exitConversations[Random.Range(0, rc.randomConversations.exitConversations.Count)];

        }
        catch
        {
            Debug.Log("random conversation list is empty");
        }
    }

    
    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        
        this._playerInteraction = playerInteraction;

        if (dialogueManager.dialogueIsPlaying || customerAI.stay != true || customerAI.hasOrdered ||
            !_canInteract) return;
        playerInteraction.pD.inUI = true;
        dialogueManager.SetCurrentCustomer(gameObject);
        DialogueManager.GetInstance().EnterDialogueMode(_introConversation);
        if (regularCustomerAtlas != null)
        {
            dialogueManager.SetPortraitButtonAndName(customerAI.customerData);
        }
        else
        {
            dialogueManager.SetDefaultImagesAndName(customerAI.customerData.name);

        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gM.pD.canMove = false;
        _customerData.customerAnimations.Talk();


    }

    public void DisplayOrderBubble()
    {
        _orderCanvas.enabled = true;
    }
    public void RemoveOrderBubble()
    {
        _orderCanvas.enabled = false;
    }

    public void DisplayOrderTicket()
    {

    }
    public void RemoveOrderTicket()
    {

    }

    public IEnumerator MoveLine()
    {
        yield return new WaitForSeconds(2);
        customerAI.customerLines[customerAI.customerLines.Count - 1].MoveLine();

    }
    public override void OnAltInteract(PlayerInteraction playerInteraction)
    {
    }

    public override void OnFocus()
    {
        
    }

    public void DeliverDrink()
    {
        gameObject.GetComponent<MoneyLauncher>().LaunchMoney((int)customerAI.customerData.orderedDrinkData.price, (int)((customerAI.customerData.orderedDrinkData.price - (int)customerAI.customerData.orderedDrinkData.price)*10));
        if (regularCustomerAtlas != null)
        {
            dialogueManager.SetPortraitButtonAndName(customerAI.customerData);
        }
        else
        {
            dialogueManager.SetDefaultImagesAndName(customerAI.customerData.name);

        }
        _playerInteraction.pD.inUI = true;
        DialogueManager.GetInstance().EnterDialogueMode(_exitConversation);
        dialogueManager.SetCurrentCustomer(gameObject);
        gM.pD.neckClamp = 0;
        dialogueManager.finishedConversation = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gM.pD.canMove = false;
        gM.pD.canMove = false;
        _customerData.customerAnimations.Talk();

    }
}
