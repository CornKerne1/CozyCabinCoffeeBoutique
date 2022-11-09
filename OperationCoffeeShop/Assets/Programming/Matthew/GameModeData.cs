using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

//This creates a file on the disk for this to be stored in the .asset format
[CreateAssetMenu(fileName = "GameModeData", menuName = "GameModeData/Generic")]
//The class does not inherit from MonoBehavior, since it it a Scriptable Object
public class GameModeData : ScriptableObject
{
    public void OnEnable()
    {
        isOpen = false;
        var c = screenShots.Count;
        screenShots = new List<Texture2D>();
        for (int i = 0; i <= c; i++)
        {
            if (File.Exists(Application.persistentDataPath + "ScreenShot" + i + ".png"))
            {
                byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + "ScreenShot" + i + ".png");
                var sS = new Texture2D(0, 0);
                sS.LoadImage(textureBytes);
                sS.filterMode = FilterMode.Point;
                screenShots.Insert(i,sS);
            }
        }
    }

    [Header("Day Night Cycle")] public float timeRate;
    public DateTime currentTime;
    public int sleepDay;
    public DateTime startTime;
    public int closingHour;
    public int wakeUpHour = 6;


    [Range(0, 16)] public int hoursOpen;

    [FormerlySerializedAs("InTutorial")] [Header("Tutorial")]
    public bool inTutorial;


    [Header("Physics Related")] public float breakSpeed = 15f;

    [Header("DO NOT TOUCH")] public bool sleeping;
    public float reputation;
    public int displayTime;
    public int day = 1;
    public float currentOpenTime;
    public bool isOpen;
    public List<Texture2D> screenShots = new List<Texture2D>();

    public string time;

    [SerializeField] public List<Material> treeMats = new List<Material>();
}