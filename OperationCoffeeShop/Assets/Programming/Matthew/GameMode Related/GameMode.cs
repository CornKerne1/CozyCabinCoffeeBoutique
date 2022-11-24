using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

    [SerializeField] private Gate gate;

    //This is the Scriptable Object that contains the data for this class.
    [FormerlySerializedAs("GameModeData")] [FormerlySerializedAs("gMD")] public GameModeData gameModeData;

    //This is a component that does not inherit from monobehavior. This class calls logic within that component. 
    public DayNightCycle DayNightCycle;
    public static event EventHandler ShopClosed;
    public static event EventHandler SurpriseCustomers;
    public static event EventHandler SaveGameEvent;

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
    public SaveOptionsData saveOptionsData;
    
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
    }

    private void Update()
    {
        //Handles the timer when the store is open.
        DayNightCycle.HandleDnc();
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
        if (saveOptionsData == null)
        {
            saveOptionsData = new SaveOptionsData();
            var gameNumber = 0;
            if (File.Exists(Application.persistentDataPath +$"SaveOptions{gameNumber}.json"))
            {
                using (StreamReader streamReader = new StreamReader(Application.persistentDataPath +$"SaveOptions{gameNumber}.json"))
                {
                    var json = streamReader.ReadToEnd();
                    saveOptionsData = JsonUtility.FromJson<SaveOptionsData>(json);
                }
                AkSoundEngine.SetRTPCValue("MasterVolume", saveOptionsData.masterVol);
                AkSoundEngine.SetRTPCValue("MusicVolume", saveOptionsData.musicVol);
                AkSoundEngine.SetRTPCValue("SFXVolume", saveOptionsData.masterVol);
            }
            else
            {
                AkSoundEngine.SetRTPCValue("MasterVolume", .4f);
                AkSoundEngine.SetRTPCValue("MusicVolume", .4f);
                AkSoundEngine.SetRTPCValue("SFXVolume", .4f);
            }
        }
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
        if (gameModeData.currentTime.Hour == 6 && gameModeData.deliveryQueued)
        {
            StartCoroutine(CO_StartDelivery());
        }
    }

    private IEnumerator CO_StartDelivery()
    {
        var truck =Instantiate(gameModeData._deliveryPrefabs.truckPrefab,Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(8f);
        SpawnDeliveryBox();
        yield return new WaitForSeconds(8f);
        Destroy(truck);
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
        var sS = ScaleTexture(ScreenCapture.CaptureScreenshotAsTexture(), 256,256);
        byte[] textureBytes = sS.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "ScreenShot" + i + ".png", textureBytes);
    }

    void OnEnable() => Load(0);
    public void Save(int gameNumber)
    {
        SaveGameEvent?.Invoke(null, EventArgs.Empty);
        saveGameData.deliveryPackages = new List<DeliveryPackage>();
        foreach (var d in DeliveryManager.GetQueue())
        {
            foreach (var dP in d.GetDeliveryPackages())
            {
                saveGameData.deliveryPackages.Add(dP);
            }
        }
        saveGameData.playerMoney = gameModeData.moneyInBank;
        var tooEarly = gameModeData.currentTime.Hour is >= 0 and < 6;
        var cT = gameModeData.currentTime;
        saveGameData.savedHour = tooEarly? 6 : gameModeData.currentTime.Hour;
        saveGameData.savedDay = gameModeData.currentTime.Day;
        saveGameData.savedMonth =gameModeData.currentTime.Month;
        saveGameData.savedYear =gameModeData.currentTime.Year;
        var json = JsonUtility.ToJson(saveGameData);
        SaveGameToDisk(gameNumber,"SaveGame",json);
    }

    public void Load(int gameNumber)
    {
        if (File.Exists(Application.persistentDataPath +$"SaveGame{gameNumber}.json"))
        {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath +$"SaveGame{gameNumber}.json"))
            {
                var json = streamReader.ReadToEnd();
                saveGameData = JsonUtility.FromJson<SaveGameData>(json);
            }

            var saveDate = new DateTime(saveGameData.savedYear, saveGameData.savedMonth, saveGameData.savedDay,
                saveGameData.savedHour, 0, 0);
            gameModeData.currentTime = saveDate;
            gameModeData.moneyInBank = saveGameData.playerMoney;
            DeliveryManager = new DeliveryManager(DeliveryManager, this, gameModeData);
            foreach (var dP in saveGameData.deliveryPackages)
            {
                DeliveryManager.AddToDelivery(dP);
            }
            SpawnSavedObjects();
        }
        else
        {
            gameModeData.currentTime = new DateTime(2027, 1, 1, 6, 0,0);
            DeliveryManager = new DeliveryManager(DeliveryManager, this, gameModeData);
            saveGameData = new SaveGameData
            {
                playerMoney = gameModeData.moneyInBank,
                deliveryPackages = new List<DeliveryPackage>(),
                respawnables = new List<RespawbableData>()
            };
        }
    }
    public void SaveGameToDisk(int gameNumber,string fileName,string json)
    {
        using (StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath +fileName+gameNumber+".json"))
        {
            streamWriter.Write(json);
        }
    }
    private void SpawnSavedObjects()
    {
        foreach (var r in saveGameData.respawnables)
        {
            Debug.Log(r.objType);
            switch (r.objType)
            {
                case DeliveryManager.ObjType.Coffee:
                    var obj = Instantiate(gameModeData._deliveryPrefabs.coffeeDispenserPrefab, r.position, r.rotation);
                    var disp = obj.GetComponent<Dispenser>();
                    disp.quantity = r.wildCard;
                    disp.delivered = true;
                    break;

                case DeliveryManager.ObjType.Milk:
                    var obj1 = Instantiate(gameModeData._deliveryPrefabs.milkDispenserPrefab, r.position, r.rotation);
                    var disp1 = obj1.GetComponent<IngredientContainer>();
                    disp1.delivered = true;
                    break;

                case DeliveryManager.ObjType.Espresso:
                    var obj2 = Instantiate(gameModeData._deliveryPrefabs.espressoDispenserPrefab, r.position, r.rotation);
                    var disp2 = obj2.GetComponent<Dispenser>();
                    disp2.quantity = r.wildCard;
                    disp2.delivered = true;
                    break;

                case DeliveryManager.ObjType.Sugar:
                    var obj3 = Instantiate(gameModeData._deliveryPrefabs.sugarDispenserPrefab, r.position, r.rotation);
                    var disp3 = obj3.GetComponent<Dispenser>();
                    disp3.quantity = r.wildCard;
                    disp3.delivered = true;
                    break;
                case DeliveryManager.ObjType.Camera:
                    var obj4 = Instantiate(gameModeData._deliveryPrefabs.cameraPrefab, r.position, r.rotation);
                    var disp4 = obj4.GetComponent<Interactable>();
                    disp4.delivered = true;
                    break;
                case DeliveryManager.ObjType.PictureFrame:
                    var obj5 = Instantiate(gameModeData._deliveryPrefabs.pictureFramePrefab, r.position, r.rotation);
                    var disp5 = obj5.GetComponent<PictureFrame>();
                    disp5.currentPic= r.wildCard;
                    disp5.delivered = true;
                    break;
            }
        }
        saveGameData.respawnables = new List<RespawbableData>();
    }
}