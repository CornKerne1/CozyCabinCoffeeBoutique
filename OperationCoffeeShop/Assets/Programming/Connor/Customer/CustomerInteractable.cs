using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomerInteractable : Interactable
{
    [Header("Visual Cue")] private TextAsset _introConversation;
    private TextAsset _exitConversationPositive;
    private TextAsset _exitConversationNegative;
    [SerializeField]private bool temporaryCarnivalGameSolution;

    [FormerlySerializedAs("CAI")] public CustomerAI customerAI;

    private GameObject _prompt;


    public Canvas canvas;

    private Canvas _orderCanvas;

    private GameObject _player;

    private CustomerData _customerData;

    private CustomerLine _line;
    


    [FormerlySerializedAs("_canInteract")] public bool canInteract = true;

    public DialogueManager dialogueManager;

    //[HideInInspector]
    [FormerlySerializedAs("rCA")] public RegularCustomerAtlas regularCustomerAtlas;

    private PlayerInteraction _playerInteraction;

    public override void Start()
    {
        base.Start();
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _customerData = gameObject.GetComponent<Customer>().customerData;
        StartCoroutine(CO_AddSelfToData());
        _orderCanvas = gameObject.GetComponentInChildren<Canvas>();
        _orderCanvas.enabled = false;
        _player = gameMode.player.gameObject;
        _playerInteraction = _player.GetComponent<PlayerInteraction>();
        customerAI = gameObject.GetComponent<CustomerAI>();

        dialogueManager = DialogueManager.GetInstance();

        // will be null if random customer or not spawned by a regular spawner
        if (regularCustomerAtlas != null)
            StartCoroutine(CO_SetConversations());
        else
        {
            StartCoroutine(SetRandomConversations());
        }
    }

    private void Update()
    {
    }

    private IEnumerator CO_AddSelfToData()
    {
        yield return new WaitForSeconds(.2f);
        _customerData.customerInteractable = this;
    }

    private IEnumerator CO_SetConversations()
    {
        yield return new WaitForSeconds(.4f);
        if (!regularCustomerAtlas.dic.ContainsKey(gameMode.gameModeData.currentTime.Day)) yield break;

        foreach (var cc in regularCustomerAtlas.dic[gameMode.gameModeData.currentTime.Day].Where(cc =>
                     cc.customer.GetComponent<Customer>().customerData.name.Equals(_customerData.name)))
        {
            _introConversation = cc.IntroConversation;
            _exitConversationPositive = cc.ExitConversationPositive;
            _exitConversationNegative = cc.ExitConversationNegative;
            break;
        }
    }

    private IEnumerator SetRandomConversations()
    {
        yield return new WaitForSeconds(2);
        Customer rc = gameObject.GetComponent<RandomCustomer>();
        try
        {
            _introConversation =
                rc.randomConversations.introConversations[
                    Random.Range(0, rc.randomConversations.introConversations.Count)];
            _exitConversationPositive =
                rc.randomConversations.exitConversationsPositive[
                    Random.Range(0, rc.randomConversations.exitConversationsPositive.Count)];
            _exitConversationNegative =
                rc.randomConversations.exitConversationsNegative[
                    Random.Range(0, rc.randomConversations.exitConversationsNegative.Count)];
        }
        catch
        {
            Debug.Log("random conversation list is empty");
        }
    }


    public override async void OnInteract(PlayerInteraction interaction)
    {
        this._playerInteraction = interaction;
        if (dialogueManager.dialogueIsPlaying || customerAI.stay != true || customerAI.hasOrdered ||
            !canInteract) return;
        interaction.playerData.inUI = true;
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
        gameMode.playerData.canMove = false;
        _customerData.customerAnimations.Talk();
        Speak();
        if (temporaryCarnivalGameSolution)
            await gameMode.SpawnCarnivalTruck();
    }

    public void Speak()
    {
        AkSoundEngine.PostEvent(_customerData.soundEngineEnvent, gameObject);
    }

    public void DisplayOrderBubble()
    {
        _orderCanvas.enabled = true;
        _orderCanvas.sortingLayerName = "WaterFall";
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
        Debug.Log(gameObject + " is getting out of line: " +
                  customerAI.customerLines[customerAI.customerLines.Count - 1]);
        yield return new WaitForSeconds(2);
        customerAI.customerLines[customerAI.customerLines.Count - 1].MoveLine();
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
    }

    public override void OnFocus()
    {
    }

    public void DeliverDrink(bool isGoodDrink)
    {
        gameObject.GetComponent<MoneyLauncher>().LaunchMoney((int)customerAI.customerData.orderedDrinkData.price,
            (int)((customerAI.customerData.orderedDrinkData.price -
                   (int)customerAI.customerData.orderedDrinkData.price) * 10));
        if (regularCustomerAtlas != null)
        {
            dialogueManager.SetPortraitButtonAndName(customerAI.customerData);
        }
        else
        {
            dialogueManager.SetDefaultImagesAndName(customerAI.customerData.name);
        }

        _playerInteraction.playerData.inUI = true;
        DialogueManager.GetInstance()
            .EnterDialogueMode(isGoodDrink ? _exitConversationPositive : _exitConversationNegative);
        dialogueManager.SetCurrentCustomer(gameObject);
        gameMode.playerData.neckClamp = 0;
        dialogueManager.finishedConversation = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameMode.playerData.canMove = false;
        gameMode.playerData.canMove = false;
        _customerData.customerAnimations.Talk();
        Speak();
    }
}