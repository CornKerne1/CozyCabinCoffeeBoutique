using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeData", menuName = "GameModeData/Generic")]
public class GameModeData : ScriptableObject
{
    [Header("Variables to Play With")]
    public float openTimer;

    [Header("DO NOT TOUCH")]
    public int timeOfDay;
    public float currentOpenTime;
    public bool isOpen;
    public GameMode gameModeInstance;

}
