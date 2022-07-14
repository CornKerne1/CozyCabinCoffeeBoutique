using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Serialization;


public class Objectives : MonoBehaviour
{
    public List<ObjectiveStruct> objectives;

    public TextMeshProUGUI textMesh;

    public int currentObjective;

    private Outline _outline;
    private Color _prevOutlineColor;


    private void Start()
    {
        textMesh.text = objectives[currentObjective].objectiveText;
    }

    private void Update()
    {
        if (objectives[currentObjective].useOutline && _outline.OutlineColor != Color.green)
        {
            _outline.OutlineColor = Color.green;
        }
    }

    public void NextObjective(GameObject sender)
    {
        var objectiveStruct = objectives[currentObjective];

        if (objectiveStruct.useOutline)
        {
            _outline.OutlineColor = _prevOutlineColor;
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            objectiveStruct.objectiveComplete = true;
        }

        Debug.Log("" + ((GameObject)sender).name);
        if (sender != objectiveStruct.triggerGameObject || currentObjective + 1 >= objectives.Count) return;
        AkSoundEngine.PostEvent("PLAY_TASKCOMPLETE", gameObject);
        if (objectives[++currentObjective].useOutline)
        {
            _outline = objectives[currentObjective].outlineGameObject.GetComponent<Outline>();
            _prevOutlineColor = _outline.OutlineColor;
            _prevOutlineColor.a = 0;
            _outline.OutlineColor = Color.green;
            _outline.OutlineMode = Outline.Mode.OutlineAll;
        }
        else
        {
            _prevOutlineColor = _outline.OutlineColor;
            _prevOutlineColor.a = 0;
        }

        textMesh.text = objectives[currentObjective].objectiveText;
    }

    [Serializable]
    public struct ObjectiveStruct
    {
        public GameObject triggerGameObject;
        public GameObject outlineGameObject;
        public bool useOutline;
        [TextArea] public string objectiveText;
        public bool objectiveComplete;
    }
}