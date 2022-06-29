using System;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial
{
    //going to have all the logic needed by any tutorial object
    //it will be constructed by the game mode (game mode will have a bool in GameModeData for tutorial)
    private bool _completedObjective;
    private Objectives1 _objectives1;
    private Tutorial _tutorial;
    private GameMode _gameMode;
    private GameModeData _gameModeData;

    public GameObject CupDispenserGameObject;
    public Dispenser CupDispenser;
    public GameObject FilterDispenserGameObject;
    public Dispenser FilterDispenser;
    public GameObject CoffeeDispenserGameObject;
    public Dispenser CoffeeDispenser;
    public GameObject BrewerBowlGameObject;
    public BrewerBowl BrewerBowl;
    public GameObject WalkInKitchenTriggerGameObject;
    public WalkInKitchenTrigger WalkInKitchenTrigger;

    public TextMeshProUGUI textMesh;
    public int currentObjective;
    
    public Tutorial(Tutorial tutorial, GameMode gameMode, GameModeData gameModeData)
    {
        _tutorial = tutorial;
        _gameMode = gameMode;
        _gameModeData = gameModeData;
    }

    public void Initialize()
    {
        
    }
    

    public void AddedGameObject(GameObject gameObject)
    {
        try
        {
            var dispenser = gameObject.GetComponent<Dispenser>();
            switch (dispenser.objType.dispenserType)
            {
                case DispenserType.Cup:
                    CupDispenser = dispenser;
                    break;
                case DispenserType.Filter:
                    FilterDispenser = dispenser;
                    break;
                case DispenserType.Coffee:
                    CoffeeDispenser = dispenser;
                    break;
                case DispenserType.Espresso:
                case DispenserType.Sugar:
                case DispenserType.Tea:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch
        {
            try
            {
                WalkInKitchenTrigger = gameObject.GetComponent<WalkInKitchenTrigger>();
            }
            catch
            {
                try
                {
                    BrewerBowl = gameObject.GetComponent<BrewerBowl>();
                }
                catch
                {
                }
            }
        }
    }

    public void NextObjective(GameObject gameObject)
    {
        _objectives1.NextObjective(gameObject);
    }

    // private void IfTutorialObjective()
    // {
    //     if (_inTutorial && !_completedObjective)
    //     {
    //         _objectives1.NextObjective(gameObject);
    //         _completedObjective = true;
    //     }
    // }
}