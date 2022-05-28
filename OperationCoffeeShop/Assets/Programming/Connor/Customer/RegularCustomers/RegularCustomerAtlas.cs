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
        public List<GameObject> customers;
    }

    public List<KeyValuePair> customerDays = new List<KeyValuePair>();
    public Dictionary<int, List<GameObject>> dic = new Dictionary<int, List<GameObject>>();

    public void updateDictionary()
    {
        foreach (var kvp in customerDays)
        {
            dic[kvp.Day] = kvp.customers;
        }
    }

    
    
}

