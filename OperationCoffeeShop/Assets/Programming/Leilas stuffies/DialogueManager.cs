using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
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
}



