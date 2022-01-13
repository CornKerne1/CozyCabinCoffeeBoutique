using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This creates a file on the disk for this to be stored in the .asset format
[CreateAssetMenu(fileName = "GameModeData", menuName = "GameModeData/Generic")]
//The class does not inherit from MonoBehavior, since it it a Scriptable Object
public class GameModeData : ScriptableObject
{
    //These are all my variables for a particular component or system
    
    //You can categorize your variables with a header.
    [Header("Day Night Cycle")]
    //You can use a slider with the RangeAttribute
    [Range(0f, 720f)]
    public float openTimer;


    [Range(0, 16)]
    public int hoursOpen;

    //This section is for variable you do not want designers to play with.
    [Header("DO NOT TOUCH")]
    public int reputation;
    public int displayTime;
    public int timeOfDay;
    public float currentOpenTime;
    public bool isOpen;

}
