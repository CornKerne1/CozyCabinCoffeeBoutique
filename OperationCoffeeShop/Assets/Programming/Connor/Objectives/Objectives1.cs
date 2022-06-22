using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class Objectives1 : MonoBehaviour
{
    public ObjectiveObjects objectives;

    public TextMeshProUGUI textMesh;

    
    public static event EventHandler ObjectiveComplete;

    public int currentObjective;
    
    private void Start()
    {
        textMesh.text = objectives[currentObjective];
        ObjectiveComplete += NextObjective;
    }

    private void NextObjective(object sender, EventArgs e)
    {
        if (currentObjective == (int) sender)
        {
            currentObjective++;
            textMesh.text = objectives[currentObjective];
        }
    }
}