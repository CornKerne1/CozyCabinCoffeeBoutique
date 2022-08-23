using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Customer", menuName = "Customer/RandomConversations")]
public class RandomConversations : ScriptableObject
{
    public List<TextAsset> introConversations;
    [FormerlySerializedAs("exitConversations")] public List<TextAsset> exitConversationsPositive;
    public List<TextAsset> exitConversationsNegative;

}
