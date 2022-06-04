using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private Image portrait;
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

    private static DialogueManager instance;
    private Story currentStory;
    private List<string> currentTags;

    public bool dialogueIsPlaying;
    public bool finishedConversation = false;

    public GameObject currentCustoemr;



    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

    }



    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        currentTags = currentStory.currentTags;
        ApplyTags();
        ContinueStory();
        if (currentStory.currentChoices.Count > 0)
        {
            button1Text.text = currentStory.currentChoices[0].text;
        }
        else
        {
            button1Text.text = "Continue";
        }
    }

    private void ApplyTags()
    {
        foreach (string s in currentTags)
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
                    if (portraitNeutral!= null)
                    {
                        portrait.sprite = portraitNeutral;
                    }
                    break;

                default:

                    break;
            }
        }
    }

    public void ExitDialogueMode()
    {
        Debug.Log("Exiting dialogue");
        CustomerAI cAI = currentCustoemr.GetComponent<CustomerAI>();
        CustomerInteractable cI = currentCustoemr.GetComponent<CustomerInteractable>();
        if (!cAI.hasOrdered)
        {
            cAI.hasOrdered = true;
            StartCoroutine(cI.MoveLine());
            cI.DisplayOrderBubble();
            cI.DisplayOrderTicket();
        }

        finishedConversation = true;
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {

            dialogueText.text = currentStory.Continue();
        }
        else if (currentStory.currentChoices.Count > 0)
        {
            currentStory.ChooseChoiceIndex(0);

            dialogueText.text = currentStory.Continue();
            if (currentStory.currentChoices.Count > 0)
            {
                button1Text.text = currentStory.currentChoices[0].text;
            }
            else
            {
                button1Text.text = "Continue";
            }

        }
        else
        {
            Debug.Log("we are now exiting dialgoue");
            ExitDialogueMode();
        }
        string dialogue = dialogueText.text;
        if (dialogue.Trim() == "" && dialogueIsPlaying)
        {
            ExitDialogueMode();
        }
    }
    public GameObject GetCurrentCustomer()
    {
        return currentCustoemr;
    }
    public void SetCurrentCustomer(GameObject customer)
    {
        currentCustoemr = customer;
    }
    public void SetPortraitButtonAndName(CustomerData CD)
    {
        portraitAmazed = CD.portraitAmazed;
        portraitAnnoyed = CD.protraitAnoyed;
        portraitHappy = CD.portraitHappy;
        portraitNeutral = CD.portraitNeutral;
        portrait.sprite = CD.portraitNeutral;
        buttonImage.sprite = CD.buttonImage;
        dialogueBoxImage.sprite = CD.dialogueBoxImage;
        displayName.text = CD.name;
    }
    public void SetDefaultImagesAndName(string name)
    {
        portraitAmazed = null;
        portraitAnnoyed = null;
        portraitHappy = null;
        portraitNeutral = null;
        portrait.sprite = defaultPortrait;
        buttonImage.sprite = defaultButtonImage;
        dialogueBoxImage.sprite = defaultDialogueBox;

        displayName.text = name;

    }

}



