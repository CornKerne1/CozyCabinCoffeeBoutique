using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "ConnorConversation/Generic")]
public class ConnorConverstation : ScriptableObject
{




    [SerializeField]
    public string customerName = "";

    public ConversationTree conversationTree;


    [System.Serializable]
    public class ConversationTree
    {
        [TextArea]
        public string message;
        public Mood mood;

        [SerializeField, Header("Can be left empty for no option")]
        public string PlayerOptionA;
        public string PlayerOptionB;
        [SerializeField, Header("ONLY FIRST ITEM MATTERS")]
        public List<ConversationTree> ATree;
        public List<ConversationTree> BTree;

    }

    public enum Mood
    {
        Happy, Sad, Neutral
    }

}
