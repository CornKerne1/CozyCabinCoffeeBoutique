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
    [SerializeField] private Sprite defaultPortrait;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite defaultButtonImage;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI button1Text;
    [SerializeField] private TextMeshProUGUI displayName;
    private static DialogueManager instance;
    private Story currentStory;

    public bool dialogueIsPlaying;
    public bool finishedConversation = false;

    private GameObject currentCustoemr;



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

    private void ExitDialogueMode()
    {
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
    }
    public GameObject GetCurrentCustomer()
    {
        return currentCustoemr;
    }
    public void SetCurrentCustomer(GameObject customer)
    {
        currentCustoemr = customer;
    }
    public void SetPortraitButtonAndName(Sprite portrait, Sprite buttonImage, string name)
    {
        this.portrait.sprite = portrait;
        this.buttonImage.sprite = buttonImage;
        displayName.text = name;
    }
    public void SetDefaultImagesAndName(string name)
    {
        portrait.sprite = defaultPortrait;
        buttonImage.sprite = defaultButtonImage;
        displayName.text = name;

    }

}



