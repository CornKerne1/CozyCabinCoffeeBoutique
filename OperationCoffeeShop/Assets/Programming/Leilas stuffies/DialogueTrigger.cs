using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DialogueTrigger : Interactable
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualcue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake()
    {
        base.Awake();
        playerInRange = false;
        visualcue.SetActive(false);
    }



    private void onTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }

    public override void OnFocus()
    {
        visualcue.SetActive(true);
    }

    public override void OnLoseFocus()
    {
        visualcue.SetActive(false);
    }
}



