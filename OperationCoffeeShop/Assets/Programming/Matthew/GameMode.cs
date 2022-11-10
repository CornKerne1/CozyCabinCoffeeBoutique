using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class GameMode : MonoBehaviour
{
    //This class keeps track of the game
    [SerializeField] protected float defaultTimeRate;
    [SerializeField] public Transform player;
    [SerializeField] public Camera camera;
    [FormerlySerializedAs("pD")] [SerializeField] public PlayerData playerData;
    [SerializeField] public GameObject playerPref;
    [SerializeField] public ScriptableOptions scriptableOptions;

    [SerializeField] private Gate gate;

    //This is the Scriptable Object that contains the data for this class.
    [FormerlySerializedAs("GameModeData")] [FormerlySerializedAs("gMD")] public GameModeData gameModeData;

    //This is a component that does not inherit from monobehavior. This class calls logic within that component. 
    public DayNightCycle DayNightCycle;
    public static event EventHandler ShopClosed;
    public static event EventHandler SurpriseCustomers;


    private List<GameObject> _toBeDestroyed = new List<GameObject>();


    [SerializeField] public GameObject sunLight;

    [FormerlySerializedAs("GameOver")] [SerializeField]
    private GameObject gameOver;

    static uint[] playingIds = new uint[50];

    [Header("Tutorial Stuffs")] public Tutorial Tutorial;
    [FormerlySerializedAs("Objectives")] public Objectives objectives;


    protected void Start()
    {
        playerData = player.GetComponent<PlayerInteraction>().playerData;
        playerData.moveSpeed = playerData.closeSpeed;
        gameModeData.timeRate = defaultTimeRate;
        camera = player.GetComponentInChildren<Camera>();
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

    protected virtual void Initialize()
    {
        //if save file exists load DateTime from file else set to startTime
        DayNightCycle.Initialize();
        gameModeData.startTime =
            new DateTime(2027, 1, 1, 5, 30, 0); //gMD.startTime = new DateTime(2027, 1, 1, 5, 0, 0);
        gameModeData.currentTime = gameModeData.startTime;
        AkSoundEngine.SetRTPCValue("MasterVolume", scriptableOptions.masterVol);
        AkSoundEngine.SetRTPCValue("MusicVolume", scriptableOptions.musicVol);
        AkSoundEngine.SetRTPCValue("SFXVolume", scriptableOptions.masterVol);
    }

    private void IfTutorial()
    {
        if (!gameModeData.inTutorial) return;
        AkSoundEngine.PostEvent("PLAY_DREAMSCAPE_", gameObject);
        Tutorial = new Tutorial(Tutorial, this, gameModeData)
        {
            Objectives = objectives
        };
    }

    public void DeactivateAndDestroy(GameObject obj)
    {
        obj.SetActive(false);
        _toBeDestroyed.Add(obj);
    }

    public void OpenShop()
    {
        if (gameModeData.currentTime.Hour is >= 18 or <= 5) return;
        gameModeData.isOpen = true;
        gate.OpenGate();
    }

    public void CloseShop()
    {
        gameModeData.isOpen = false;
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

    public void Surprise(GameObject breakableSource)
    {
        Debug.Log("suprise!!!");
        SurpriseCustomers?.Invoke(breakableSource, EventArgs.Empty);
    }

    public void TakePicture()
    {
        for (int i = 1; i <=100; i++)
        {
            if (!File.Exists(Application.persistentDataPath + "ScreenShot" + i + ".png"))
            {
                SaveScreenShot(i);
                break;
            }
        }
    }
    private static Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) 
    {
        Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
        Color[] rpixels=result.GetPixels(0);
        float incX=(1.0f / (float)targetWidth);
        float incY=(1.0f / (float)targetHeight); 
        for(int px=0; px<rpixels.Length; px++) { 
            rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth), incY*((float)Mathf.Floor(px/targetWidth))); 
        } 
        result.SetPixels(rpixels,0); 
        result.Apply(); 
        return result; 
    }

    public static Texture2D LoadTextureFromDisk(int i)
    {
        byte[] textureBytes =
            File.ReadAllBytes(Application.persistentDataPath + "ScreenShot" + i + ".png");
        var sS = new Texture2D(0, 0);
        sS.LoadImage(textureBytes);
        sS.filterMode = FilterMode.Point;
        return sS;
    }
    
    private static void SaveScreenShot(int i)
    {
        var sS = ScaleTexture(ScreenCapture.CaptureScreenshotAsTexture(), 480,270);
        byte[] textureBytes = sS.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "ScreenShot" + i + ".png", textureBytes);
    }
}