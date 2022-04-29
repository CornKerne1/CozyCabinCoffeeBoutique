using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This creates a file on the disk for this to be stored in the .asset format
[CreateAssetMenu(fileName = "GameModeData", menuName = "GameModeData/Generic")]
//The class does not inherit from MonoBehavior, since it it a Scriptable Object
public class GameModeData : ScriptableObject
{
    public void OnEnable()
    {
        isOpen = false;
    }

    [Header("Day Night Cycle")]

    public float timeRate;
    public DateTime currentTime;
    public DateTime sleepTime;
    public DateTime startTime;
    public int closingHour;
    public int wakeUpHour = 6;


    [Range(0, 16)]
    public int hoursOpen;

    [Header("DO NOT TOUCH")]
    public bool sleeping;
    public float reputation;
    public int displayTime;
    public int day = 1;
    public float currentOpenTime;
    public bool isOpen;

    public string time;
}
