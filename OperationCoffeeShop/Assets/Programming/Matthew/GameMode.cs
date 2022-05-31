using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameMode : MonoBehaviour
{
    //This class keeps track of the game

    [SerializeField]public Transform player;
    [SerializeField]public PlayerData pD;
    [SerializeField]public GameObject playerPref;
    [SerializeField]private Gate gate;
    //This is the Scriptable Object that contains the data for this class.
    public GameModeData gMD;
    //This is a component that does not inherit from monobehavior. This class calls logic within that component. 
    public DayNightCycle dNC;
    public static event EventHandler ShopClosed;

    private List<GameObject> toBeDestroyed = new List<GameObject>();
    

    [SerializeField] public GameObject sunLight;

    [SerializeField] private GameObject GameOver;

    static uint[] playingIds = new uint[50];


    // Start is called before the first frame update
    void Start()
    {
        pD = player.GetComponent<PlayerInteraction>().pD;
        pD.moveSpeed = pD.closeSpeed;
    }
    
    void Awake()
    {
        //Creates new DayNightCycle component.
        dNC = new DayNightCycle(dNC, this, gMD);
        Initialize();
        //Instantiate(sunLight);
    }
    
    //Update is called once per frame
    void Update()
    {
        //Handles the timer when the store is open.
        dNC.StartTimer();
        dNC.SleepTimer();
        dNC.RotateSun();
    }
    public void UpdateReputation(int reputation)
    {
        //gMD.reputation = reputation + gMD.reputation;
        //Call method to change reputation slider.
    }

    //This is the method to call to change the time of day.
    public void UpdateTimeOfDay(int time)
    {
        dNC.UpdateTimeOfDay(time);
    }

    public void Initialize()
    {
        //if save file exists load DateTime from file else set to startTime
        dNC.Initialize();
        gMD.startTime = new DateTime(2027, 1, 1, 6, 0, 0);
        gMD.currentTime = gMD.startTime;

    }

    public void DeactivateAndDestroy(GameObject obj)
    {
        obj.SetActive(false);
        toBeDestroyed.Add(obj);
    }

    public void OpenShop()
    {
        if (gMD.currentTime.Hour < 18 && gMD.currentTime.Hour > 5)
        {
            gMD.isOpen = true;
            gate.OpenGate();
            pD.moveSpeed = pD.openSpeed;
        }
    }
    public void CloseShop()
    {
        gMD.isOpen = false;
        pD.moveSpeed = pD.closeSpeed;
        ShopClosed?.Invoke(this, EventArgs.Empty);
        if (gMD.currentTime.Day > 2)
        {
            Instantiate(GameOver);
        }
    }



    public static bool IsEventPlayingOnGameObject(string eventName, GameObject go)
    {
        uint testEventId = AkSoundEngine.GetIDFromString(eventName);

        uint count = (uint)playingIds.Length;
        AKRESULT result = AkSoundEngine.GetPlayingIDsFromGameObject(go, ref count, playingIds);

        for (int i = 0; i < count; i++)
        {
            uint playingId = playingIds[i];
            uint eventId = AkSoundEngine.GetEventIDFromPlayingID(playingId);

            if (eventId == testEventId)
                return true;
        }

        return false;
    }
}
