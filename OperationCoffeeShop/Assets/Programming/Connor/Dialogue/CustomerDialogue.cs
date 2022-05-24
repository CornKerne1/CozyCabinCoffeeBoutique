using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CustomerDialogue : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI customerNameText;
    public TextMeshProUGUI customerMessageText;
    public UnityEngine.UI.Button ButtonA;
    private UnityEngine.UI.Image ButtonAImage;
    private TextMeshProUGUI ButtonAText;
    public UnityEngine.UI.Button ButtonB;
    private UnityEngine.UI.Image ButtonBImage;
    private TextMeshProUGUI ButtonBText;


    private GameObject myEventSystem;
    private EventSystem EventSystem;


    public ConnorConverstation converstation;

    private ConnorConverstation.ConversationTree conversationTree;

    private void Start()
    {
        canvas = this.GetComponent<Canvas>();
        ButtonAText = ButtonA.GetComponentInChildren<TextMeshProUGUI>();
        ButtonBText = ButtonB.GetComponentInChildren<TextMeshProUGUI>();
        ButtonBImage = ButtonB.gameObject.GetComponent<UnityEngine.UI.Image>();
        ButtonAImage = ButtonA.gameObject.GetComponent<UnityEngine.UI.Image>();

        conversationTree = converstation.conversationTree;

        customerNameText.text = converstation.customerName;
        ButtonAText.text = conversationTree.PlayerOptionA;
        ButtonBText.text = conversationTree.PlayerOptionB;
        customerMessageText.text = conversationTree.message;
        myEventSystem = GameObject.Find("EventSystem");
        EventSystem = myEventSystem.GetComponent<EventSystem>();
    }

    public void AdvanceConversationA()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (conversationTree.ATree.Count > 0)
        {
            conversationTree = conversationTree.ATree[0];
            customerMessageText.text = conversationTree.message;

            //Displays or hides optionA, there should always be an option A
            if (conversationTree.PlayerOptionA != "")
            {
                ButtonA.enabled = true;
                ButtonAImage.enabled = true;

                ButtonAText.text = conversationTree.PlayerOptionA;
            }
            else
            {
                ButtonA.enabled = false;
                ButtonAText.text = "";
                ButtonAImage.enabled = false;

            }

            //Display or hides option B
            if (conversationTree.PlayerOptionB != "")
            {
                ButtonB.enabled = true;
                ButtonBImage.enabled = true;
                ButtonBText.text = conversationTree.PlayerOptionB;
            }
            else
            {
                ButtonB.enabled = false;
                ButtonBText.text = "";
                ButtonBImage.enabled = false;

            }

            //catches user error and hides optionless text
            if (conversationTree.PlayerOptionA == "" && conversationTree.PlayerOptionB == "")
            {
                canvas.enabled = false;
            }
        }
        else
        {
            Debug.Log(conversationTree.ATree.Count);
            canvas.enabled = false;
        }
    }
    public void AdvanceConversationB()
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (conversationTree.BTree.Count > 0)
        {
            conversationTree = conversationTree.BTree[0];
            customerMessageText.text = conversationTree.message;

            //Displays or hides optionA, there should always be an option A
            if (conversationTree.PlayerOptionA != "")
            {

                ButtonA.enabled = true;
                ButtonAImage.enabled = true;
                ButtonAText.text = conversationTree.PlayerOptionA;
            }
            else
            {
                ButtonA.enabled = false;
                ButtonAText.text = "";
                ButtonAImage.enabled = false;

            }

            //Display or hides option B
            if (conversationTree.PlayerOptionB != "")
            {
                ButtonB.enabled = true;
                ButtonBImage.enabled = true;
                ButtonBText.text = conversationTree.PlayerOptionB;
            }
            else
            {
                ButtonB.enabled = false;
                ButtonBText.text = "";
                ButtonBImage.enabled = false;

            }

            //catches user error and hides optionless text
            if (conversationTree.PlayerOptionB == "" && conversationTree.PlayerOptionB == "")
            {
                canvas.enabled = false;
            }
        }
        else canvas.enabled = false;
    }


}
