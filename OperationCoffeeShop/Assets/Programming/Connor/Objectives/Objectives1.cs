using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class Objectives1 : MonoBehaviour
{
    public List<GameObject> objectiveObjects;
    [TextArea] public List<string> objectives;

    public TextMeshProUGUI textMesh;

    public int currentObjective;


    private void Start()
    {
        textMesh.text = objectives[currentObjective];
    }

    public void NextObjective(GameObject sender)
    {
        Debug.Log("" + ((GameObject)sender).name);
        if ((GameObject)sender != objectiveObjects[currentObjective] || currentObjective + 1 >= objectives.Count) return;
        currentObjective++;
        textMesh.text = objectives[currentObjective];
    }
}