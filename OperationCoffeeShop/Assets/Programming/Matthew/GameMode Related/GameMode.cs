using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ink.Runtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
[Serializable]
public class GameMode : MonoBehaviour,ISaveState
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
    public static event EventHandler SaveGameEvent;
    public static event EventHandler LoadGameEvent;

    public DeliveryManager DeliveryManager;
    public CoffeeBankTM CoffeeBankTM;

    private List<GameObject> _toBeDestroyed = new List<GameObject>();


    [SerializeField] public GameObject sunLight;

    [FormerlySerializedAs("GameOver")] [SerializeField]
    private GameObject gameOver;

    static uint[] playingIds = new uint[50];

    [Header("Tutorial Stuffs")] public Tutorial Tutorial;
    [FormerlySerializedAs("Objectives")] public Objectives objectives;
    
    public SaveGameData saveGameData;
    
    private void Awake()
    {
        //Creates new DayNightCycle component.
        DayNightCycle = new DayNightCycle(DayNightCycle, this, gameModeData);
        CoffeeBankTM = new CoffeeBankTM(CoffeeBankTM, this, gameModeData);
        Initialize();
        //Instantiate(sunLight);
        IfTutorial();
    }
    protected void Start()
    {
        playerData = player.GetComponent<PlayerInteraction>().playerData;
        playerData.moveSpeed = playerData.closeSpeed;
        gameModeData.timeRate = defaultTimeRate;
        camera = player.GetComponentInChildren<Camera>();
        DayNightCycle.HourChanged += CheckForDelivery;
        CheckForDelivery(this, EventArgs.Empty);
    }

    private void Update()
    {
        //Handles the timer when the store is open.
        DayNightCycle.HandleDnc();
        Debug.Log(DeliveryManager.GetQueue().Count);
    }

    public void UpdateReputation(int reputation)
    {
        //gMD.reputation = reputation + gMD.reputation;
        //Call method to change reputation slider.
    }

    protected virtual void Initialize()
    {
        //if save file exists load DateTime from file else set to startTime
        DayNightCycle.Initialize();
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
        playerData.canJump = false;
        gate.OpenGate();
    }

    public void CloseShop()
    {
        gameModeData.isOpen = false;
        playerData.canJump = true;
        ShopClosed?.Invoke(this, EventArgs.Empty);
        if (gameModeData.currentTime.Day > 2)
        {
            Instantiate(gameOver);
        }
    }
    private void CheckForDelivery(object sender, EventArgs e)
    {
        if (gameModeData.deliveryQueued)
        {
            SpawnDeliveryBox();
        }
        // if (gameModeData.currentTime.Hour == 6 && gameModeData.deliveryQueued)
        // {
        //     SpawnDeliveryBox();
        // }
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

    public void SpawnDeliveryBox()
    {
        Instantiate(gameModeData.deliveryBoxPref,gameModeData.deliveryPosition,quaternion.identity);
    }

    public void TakePicture()
    {
        for (int i = 1; i <=1000; i++)
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
        var sS = ScaleTexture(ScreenCapture.CaptureScreenshotAsTexture(), 128,128);
        byte[] textureBytes = sS.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "ScreenShot" + i + ".png", textureBytes);
    }

    void OnEnable() => Load(0);
    void OnDisable() => Save(0);

    public void Save(int gameNumber)
    {
        saveGameData.Deliveries = new List<DeliveryData>();
        for (int i = 0; i < DeliveryManager.GetQueue().Count; i++)
        {
            saveGameData.Deliveries.Add(DeliveryManager.GetQueue().Dequeue());
        }
        saveGameData.playerMoney = gameModeData.moneyInBank;
        var pastClosing = gameModeData.currentTime.TimeOfDay.Hours >= gameModeData.closingHour;
        var cT = gameModeData.currentTime;
        saveGameData.savedDate = pastClosing? new DateTime(cT.Year,cT.Month,cT.Day+1) : new DateTime(cT.Year,cT.Month,cT.Day);
        saveGameData.playerPosition = player.transform.position;
        SaveGameToDisk(gameNumber);
    }
    public void Load(int gameNumber)
    {
        if (File.Exists($"SaveGame{gameNumber}.json"))
        {
            using (StreamReader streamReader = new StreamReader($"SaveGame{gameNumber}.json"))
            {
                var json = streamReader.ReadToEnd();
                saveGameData = JsonUtility.FromJson<SaveGameData>(json);
            }
            var sD = saveGameData.savedDate;
            gameModeData.currentTime = new DateTime(sD.Year,sD.Month,sD.Day,6,0,0);
            gameModeData.moneyInBank = saveGameData.playerMoney;
            player.transform.position = saveGameData.playerPosition;
            DeliveryManager = new DeliveryManager(DeliveryManager, this, gameModeData);
            Debug.Log(saveGameData.Deliveries);
            if (saveGameData.Deliveries != null)
            {
                for (int i = 0; i < saveGameData.Deliveries.Count; i++)
                {
                    DeliveryManager.GetQueue().Enqueue(saveGameData.Deliveries[i]);
                }
            }
            
        }
        else
        {
            gameModeData.currentTime = new DateTime(2027, 1, 1, 6, 0,0);
            saveGameData = new SaveGameData
            {
                playerMoney = gameModeData.moneyInBank,
                savedDate = gameModeData.currentTime,
                playerPosition = player.transform.position,
                Deliveries = new List<DeliveryData>()
            };
        }
    }
    private void SaveGameToDisk(int gameNumber)
    {
        var json = JsonUtility.ToJson(saveGameData);
        using (StreamWriter streamWriter = new StreamWriter($"SaveGame{gameNumber}.json"))
        {
            streamWriter.Write(json);
        }
    }
    private void SpawnSavedObjects()
    {
        foreach (var r in saveGameData.respawnables)
        {
            switch (r.objType)
            {
                case DeliveryManager.ObjType.Coffee:
                    var obj = Instantiate(gameModeData._deliveryPrefabs.coffeeDispenserPrefab, r.position, r.rotation);
                    obj.GetComponent<Dispenser>().quantity = r.wildCard;
                    break;

                case DeliveryManager.ObjType.Milk:
                    var obj1 = Instantiate(gameModeData._deliveryPrefabs.milkDispenserPrefab, r.position, r.rotation);
                    break;

                case DeliveryManager.ObjType.Espresso:
                    var obj2 = Instantiate(gameModeData._deliveryPrefabs.espressoDispenserPrefab, r.position, r.rotation);
                    obj2.GetComponent<Dispenser>().quantity = r.wildCard;
                    break;

                case DeliveryManager.ObjType.Sugar:
                    var obj3 = Instantiate(gameModeData._deliveryPrefabs.sugarDispenserPrefab, r.position, r.rotation);
                    obj3.GetComponent<Dispenser>().quantity = r.wildCard;
                    break;
            }
        }
    }
}