using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeData", menuName = "GameModeData/Generic")]
public class GameModeData : ScriptableObject
{
    [Header("Variables to Play With")]
    [Range(0f, 720f)]
    public float openTimer;
    [Range(0, 16)]
    public int hoursOpen;

    [Header("DO NOT TOUCH")]
    public int displayTime;
    public int timeOfDay;
    public float currentOpenTime;
    public bool isOpen;
    public GameMode gameModeInstance;

}
