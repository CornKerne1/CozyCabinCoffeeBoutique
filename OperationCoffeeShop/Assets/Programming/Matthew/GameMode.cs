using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameMode : MonoBehaviour
{
    //This class keeps track of the game

    [SerializeField] public Transform player;
    [SerializeField] public PlayerData pD;
    [SerializeField] public GameObject playerPref;

    [SerializeField] private Gate gate;

    //This is the Scriptable Object that contains the data for this class.
    [FormerlySerializedAs("gMD")] public GameModeData gameModeData;

    //This is a component that does not inherit from monobehavior. This class calls logic within that component. 
    public DayNightCycle DayNightCycle;
    public static event EventHandler ShopClosed;

    private List<GameObject> _toBeDestroyed = new List<GameObject>();


    [SerializeField] public GameObject sunLight;

    [FormerlySerializedAs("GameOver")] [SerializeField]
    private GameObject gameOver;

    static uint[] playingIds = new uint[50];

    [Header("Tutorial Stuffs")] public Tutorial Tutorial;
    [FormerlySerializedAs("Objectives")] public Objectives objectives;

    private void Start()
    {
        pD = player.GetComponent<PlayerInteraction>().pD;
        pD.moveSpeed = pD.closeSpeed;
    }

    private void Awake()
    {
        //Creates new DayNightCycle component.
        DayNightCycle = new DayNightCycle(DayNightCycle, this, gameModeData);
        Initialize();
        //Instantiate(sunLight);
        IfTutorial();
    }

    private void Update()
    {
        //Handles the timer when the store is open.
        DayNightCycle.StartTimer();
        DayNightCycle.SleepTimer();
        DayNightCycle.RotateSun();
    }

    public void UpdateReputation(int reputation)
    {
        //gMD.reputation = reputation + gMD.reputation;
        //Call method to change reputation slider.
    }

    //This is the method to call to change the time of day.
    public void UpdateTimeOfDay(int time)
    {
        DayNightCycle.UpdateTimeOfDay(time);
    }

    private void Initialize()
    {
        //if save file exists load DateTime from file else set to startTime
        DayNightCycle.Initialize();
        gameModeData.startTime =
            new DateTime(2027, 1, 1, 5, 30, 0); //gMD.startTime = new DateTime(2027, 1, 1, 5, 0, 0);
        gameModeData.currentTime = gameModeData.startTime;
    }

    private void IfTutorial()
    {
        if (gameModeData.inTutorial)
        {
            AkSoundEngine.PostEvent("PLAY_DREAMSCAPE_", gameObject);
            Tutorial = new Tutorial(Tutorial, this, gameModeData)
            {
                Objectives = objectives
            };
        }
    }

    public void DeactivateAndDestroy(GameObject obj)
    {
        obj.SetActive(false);
        _toBeDestroyed.Add(obj);
    }

    public void OpenShop()
    {
        if (gameModeData.currentTime.Hour >= 18 || gameModeData.currentTime.Hour <= 5) return;
        gameModeData.isOpen = true;
        gate.OpenGate();
        pD.moveSpeed = pD.openSpeed;
    }

    public void CloseShop()
    {
        gameModeData.isOpen = false;
        pD.moveSpeed = pD.closeSpeed;
        ShopClosed?.Invoke(this, EventArgs.Empty);
        if (gameModeData.currentTime.Day > 2)
        {
            Instantiate(gameOver);
        }
    }


    public static bool IsEventPlayingOnGameObject(string eventName, GameObject go)
    {
        var testEventId = AkSoundEngine.GetIDFromString(eventName);

        var count = (uint)playingIds.Length;
        var result = AkSoundEngine.GetPlayingIDsFromGameObject(go, ref count, playingIds);

        for (var i = 0; i < count; i++)
        {
            var playingId = playingIds[i];
            var eventId = AkSoundEngine.GetEventIDFromPlayingID(playingId);

            if (eventId == testEventId)
                return true;
        }

        return false;
    }
}