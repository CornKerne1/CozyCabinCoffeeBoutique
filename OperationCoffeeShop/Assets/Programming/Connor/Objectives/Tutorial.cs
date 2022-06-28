using System.Collections;
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
    

    public Tutorial(Tutorial tutorial, GameMode gameMode, GameModeData gameModeData)
    {
        _tutorial = tutorial;
        _gameMode = gameMode;
        _gameModeData = gameModeData;
    }
    
    
    private void IfTutorial()
    {
        try
        {
            _inTutorial = true;
        }
        catch
        {
            // ignored
        }
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
