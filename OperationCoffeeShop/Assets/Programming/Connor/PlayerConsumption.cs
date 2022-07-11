using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumption : MonoBehaviour
{
    public GameMode gameMode;
    
    [SerializeField,Header("Tutorial Stuff")]
    private Objectives1 _objectives1;
    private bool _completedObjective;
    
    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

      SetTutorial();
    }
    private void SetTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            _objectives1 = gameMode.Tutorial.Objectives;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<LiquidIngredients>(out var liquid))
        {
            liquid.gameObject.SetActive(false); 
            Debug.Log("Drinking the coffee");
            IfTutorial();
        }
    }

    private void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial && !_completedObjective)
        {
            _completedObjective = true;
            _objectives1.NextObjective(gameObject);

        }
    }
}