using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Serialization;


public class Objectives : MonoBehaviour
{
    public List<ObjectiveStruct> objectives;

    public TextMeshProUGUI textMesh;

    public int currentObjective;

    private Outline _outline;
    private Color _prevOutlineColor;

    [SerializeField] private GameObject tutorialStuckMessage;
    private Animator _stuckMessageAnimator;
    private Task _tutorialStuckTask;
    private bool destroyed;


    private async void Start()
    {
        textMesh.text = objectives[currentObjective].objectiveText;
        await Task.Delay(300*1000);
        HandleTutorialStuckMessage();
    }

    private void Update()
    {
        if (objectives[currentObjective].useOutline && (_outline.OutlineColor != Color.green || !_outline.enabled) )
        {
            _outline.enabled = true;
            _outline.OutlineColor = Color.green;
            _outline.OutlineMode = Outline.Mode.OutlineAll;
        }
    }

    public async void HandleTutorialStuckMessage()
    {
        if (destroyed) return;
        if (_stuckMessageAnimator == null)
        {
            tutorialStuckMessage.SetActive(true);
            _stuckMessageAnimator = tutorialStuckMessage.GetComponent<Animator>();
            _stuckMessageAnimator.enabled=false;
        }

        _tutorialStuckTask = StuckMessage();
        await _tutorialStuckTask;
    }

    private async Task StuckMessage()
    {
        if (destroyed) return;
        _stuckMessageAnimator.enabled=true;
        await Task.Delay(5000);
        if (destroyed) return;
        _stuckMessageAnimator.enabled=false;
        _stuckMessageAnimator.enabled=true;
        await Task.Delay(5000);
        if (destroyed) return;
        _stuckMessageAnimator.enabled=false;
        await Task.Delay(15000);
        if (destroyed) return;
        _tutorialStuckTask = null;
        HandleTutorialStuckMessage();
    }

    public bool CheckObjective(int objective)
    {
        return currentObjective == objective;
    }

    public void NextObjective(GameObject sender)
    {
        var objectiveStruct = objectives[currentObjective];

        if (objectiveStruct.useOutline)
        {
            _outline.OutlineColor = _prevOutlineColor;
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            objectiveStruct.objectiveComplete = true;
            _outline.enabled = false;
        }

        Debug.Log("" + ((GameObject)sender).name);
        if (sender != objectiveStruct.triggerGameObject || currentObjective + 1 >= objectives.Count) return;
        Debug.Log(objectives[currentObjective].objectiveText);
        AkSoundEngine.PostEvent("PLAY_TASKCOMPLETE", gameObject);
        if (objectives[++currentObjective].useOutline)
        {
            textMesh.text = objectives[currentObjective].objectiveText;
            _outline = objectives[currentObjective].outlineGameObject.GetComponent<Outline>();
            _outline.enabled = true;
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

    private void OnDestroy()
    {
        destroyed = true;
    }
}