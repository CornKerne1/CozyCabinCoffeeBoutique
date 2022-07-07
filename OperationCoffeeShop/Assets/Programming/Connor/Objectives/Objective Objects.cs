using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ObjectiveObjects", menuName = "Objective/Generic")]
public class ObjectiveObjects : ScriptableObject
{
    public List<GameObject> objectiveCallerObjectOrder;

    [TextArea] public List<string> objectives;

    public string this[int i] => objectives[i];
}