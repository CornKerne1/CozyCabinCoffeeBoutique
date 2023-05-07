using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")] [SerializeField]
    private Image portrait;

    [SerializeField] private Image buttonImage;
    [SerializeField] private Image dialogueBoxImage;


    [SerializeField] private Sprite portraitNeutral;
    [SerializeField] private Sprite portraitHappy;
    [SerializeField] private Sprite portraitAmazed;
    [SerializeField] private Sprite portraitAnnoyed;

    [SerializeField] private Sprite defaultPortrait;
    [SerializeField] private Sprite defaultButtonImage;
    [SerializeField] private Sprite defaultDialogueBox;


    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI button1Text;
    [SerializeField] private TextMeshProUGUI displayName;

    private static DialogueManager _instance;
    private Story _currentStory;
    private List<string> _currentTags;


    private bool _firstMessage = true;
    public bool dialogueIsPlaying;
    public bool finishedConversation;

    public GameObject currentCustomer;

    private Camera _camera;
    public GameMode gameMode;
    private Transform _oldLook;
    private GameObject _player;
    public Transform lookAt;


    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }

        _instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return _instance;
    }

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        _player = gameMode.player.gameObject;

        _camera = Camera.main;
    }


    private void Update()
    {
        if (gameMode.playerData.canMove || GetCurrentCustomer() != gameObject ||
            !dialogueIsPlaying) return;
        gameMode.playerData.neckClamp = 0;
        var c = _camera.transform;
        _oldLook = c;
        _camera.transform.LookAt(lookAt.position);
    }


    private void FinishedConversation()
    {
        Debug.Log("testing my patience");

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameMode.playerData.canMove = true;
        gameMode.playerData.canMove = true;
        //StartCoroutine(MoveLine());
        finishedConversation = false;
        gameMode.playerData.neckClamp = 77.3f;
        gameMode.playerData.inUI = false;
    }


    public void EnterDialogueMode(TextAsset inkJson)
    {

        Debug.Log("ink json" + inkJson);
        Debug.Log("ink json.text" + inkJson.text);

        _currentStory = new Story(inkJson.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        _currentTags = _currentStory.currentTags;
        ApplyTags();
        ContinueStory();
        button1Text.text = _currentStory.currentChoices.Count > 0 ? _currentStory.currentChoices[0].text : "Continue";
    }

    private void ApplyTags()
    {
        foreach (var s in _currentTags)
        {
            switch (s.ToLower().Trim())
            {
                case "portrait:happy":
                    if (portraitHappy != null)
                    {
                        portrait.sprite = portraitHappy;
                    }

                    break;

                case "portrait:annoyed":
                    if (portraitHappy != null)
                    {
                        portrait.sprite = portraitHappy;
                    }

                    break;

                case "portrait:amazed":
                    if (portraitAmazed != null)
                    {
                        portrait.sprite = portraitAmazed;
                    }

                    break;

                case "portrait:neutral":
                    if (portraitNeutral != null)
                    {
                        portrait.sprite = portraitNeutral;
                    }

                    break;
            }
        }
    }

    public void ExitDialogueMode()
    {
        if (!currentCustomer) return;
        var cAI = currentCustomer.GetComponent<CustomerAI>();
        var cI = cAI.customerData.customerInteractable;
        cI.canInteract = false;
        if (!cAI.hasOrdered)
        {
            StartCoroutine(cI.MoveLine());
            cI.DisplayOrderBubble();
            cI.DisplayOrderTicket();
        }
        else StartCoroutine(cI.MoveLine());

        finishedConversation = true;
        FinishedConversation();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentCustomer = null;
    }

    public void ContinueStory()
    {
        if (!_firstMessage) AkSoundEngine.PostEvent("PLAY_bubblepop", gameObject);
        else _firstMessage = false;
        if (_currentStory.canContinue)
        {
            dialogueText.text = _currentStory.Continue();
        }
        else if (_currentStory.currentChoices.Count > 0)
        {
            _currentStory.ChooseChoiceIndex(0);

            dialogueText.text = _currentStory.Continue();
            button1Text.text = _currentStory.currentChoices.Count > 0
                ? _currentStory.currentChoices[0].text
                : "Continue";
        }
        else
        {
            Debug.Log("we are now exiting dialgoue");
            ExitDialogueMode();
        }

        var dialogue = dialogueText.text;
        if (dialogue.Trim() == "" && dialogueIsPlaying)
        {
            ExitDialogueMode();
        }
    }

    public GameObject GetCurrentCustomer()
    {
        return currentCustomer;
    }

    public void SetCurrentCustomer(GameObject customer)
    {
        currentCustomer = customer;
    }

    public void SetPortraitButtonAndName(CustomerData customerData)
    {
        portraitAmazed = customerData.portraitAmazed;
        portraitAnnoyed = customerData.portraitAnnoyed;
        portraitHappy = customerData.portraitHappy;
        portraitNeutral = customerData.portraitNeutral;
        portrait.sprite = customerData.portraitNeutral;
        buttonImage.sprite = customerData.buttonImage;
        dialogueBoxImage.sprite = customerData.dialogueBoxImage;
        displayName.text = customerData.name;
    }

    public void SetDefaultImagesAndName(string displayName)
    {
        portraitAmazed = null;
        portraitAnnoyed = null;
        portraitHappy = null;
        portraitNeutral = null;
        portrait.sprite = defaultPortrait;
        buttonImage.sprite = defaultButtonImage;
        dialogueBoxImage.sprite = defaultDialogueBox;

        this.displayName.text = displayName;
    }
}