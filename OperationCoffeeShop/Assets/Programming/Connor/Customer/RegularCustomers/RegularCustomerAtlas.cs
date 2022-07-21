using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegularCustomerAtlas", menuName = "RegularCustomerAtlas/Generic")]
public class RegularCustomerAtlas : ScriptableObject
{
    [Serializable]
    public class KeyValuePair
    {
        public int Day;
        public List<customerConversations> customers;
    }

    public List<KeyValuePair> customerDays = new List<KeyValuePair>();
    public Dictionary<int, List<customerConversations>> dic = new Dictionary<int, List<customerConversations>>();

    public void updateDictionary()
    {
        foreach (var kvp in customerDays)
        {
            dic[kvp.Day] = kvp.customers;
        }
    }
    [Serializable]
    public struct customerConversations
    {
        public GameObject customer;
        public TextAsset IntroConversation;
        public TextAsset ExitConversation;
    }
    
}

