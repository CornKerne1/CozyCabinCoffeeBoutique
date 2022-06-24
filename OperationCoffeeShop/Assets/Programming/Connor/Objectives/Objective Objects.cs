using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveObjects", menuName = "Objective/Generic")]
public class ObjectiveObjects : ScriptableObject
{
    [TextArea] public List<string> objectives;

    public string this[int i] => objectives[i];
}