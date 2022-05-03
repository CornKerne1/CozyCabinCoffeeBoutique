using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDisplay : MonoBehaviour
{
    public Conversation conversation;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    private SpeakerUI speakerUILeft; //should always be the player
    private SpeakerUI speakerUIRight; // should always be the customer

    private int activeLineIndex = 0;

    public bool finishedOrder = false;

    private void Start()
    {

        Canvas canvas= gameObject.GetComponentInParent<Canvas>();
        canvas.enabled = false;

        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;

       

    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown())
        //{
        //    AdvanceConversation();
        //}
    }
    public void AdvanceConversation()
    {
        if (activeLineIndex == 0)
        {
            finishedOrder = false;
        }
        if (activeLineIndex < conversation.lines.Length)
        {
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            speakerUIRight.Hide();
            speakerUILeft.Hide();
            activeLineIndex = 0;
            finishedOrder = true;
        }
    }
    void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUILeft, line.text);

        }
        else
        {
            SetDialog(speakerUIRight, speakerUIRight, line.text);
        }
    }
    void SetDialog(
        SpeakerUI activeSpeakerUI,
        SpeakerUI inactiveSpeakerUI,
        string text
        )
    {
        activeSpeakerUI.Dialog = text;
        activeSpeakerUI.Show();

    }
    void Agree()
    {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        SetDialog(speakerUIRight, speakerUIRight, line.text);
    }
    void Disagree()
    {
        speakerUIRight.Hide();
        speakerUILeft.Hide();
        activeLineIndex = 0;
    }
}
