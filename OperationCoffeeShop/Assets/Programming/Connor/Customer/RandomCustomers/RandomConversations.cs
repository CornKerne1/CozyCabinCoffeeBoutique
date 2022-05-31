using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Customer/RandomConversations")]
public class RandomConversations : ScriptableObject
{
    public List<TextAsset> introConversations;
    public List<TextAsset> exitConversations;
}
