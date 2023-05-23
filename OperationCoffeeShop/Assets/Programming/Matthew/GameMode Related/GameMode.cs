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
    [SerializeField] public PlayerInput playerInput;
    [SerializeField] public GameObject playerPref;

    [SerializeField] private Gate gate;

    //This is the Scriptable Object that contains the data for this class.
    [FormerlySerializedAs("GameModeData")] [FormerlySerializedAs("gMD")] public GameModeData gameModeData;

    //This is a component that does not inherit from monobehavior. This class calls logic within that component. 
    public DayNightCycle DayNightCycle;
    public static event EventHandler ShopClosed;
    public static event EventHandler SurpriseCustomers;

    public DeliveryManager deliveryManager;
    public CoffeeBankTM CoffeeBankTM;
    public DynamicBatcher dynamicBatcher;

    public SaveSystem SaveSystem;

    private List<GameObject> _toBeDestroyed = new List<GameObject>();


    [SerializeField] public GameObject sunLight;

    [FormerlySerializedAs("GameOver")] [SerializeField]
    private GameObject gameOver;

    static uint[] playingIds = new uint[50];

    [Header("Tutorial Stuffs")] public Tutorial Tutorial;
    [FormerlySerializedAs("Objectives")] public Objectives objectives;

    private void Awake()
    {
        //Creates new DayNightCycle component.
        DayNightCycle = new DayNightCycle(DayNightCycle, this, gameModeData);
        CoffeeBankTM = new CoffeeBankTM(CoffeeBankTM, this, gameModeData);
        SaveSystem = new SaveSystem(SaveSystem, this, gameModeData);
        Initialize();
        //Instantiate(sunLight);
        IfTutorial();
    }
    protected async void Start()
    {
        var playerInteraction = player.GetComponent<PlayerInteraction>();
        playerInput = playerInteraction.playerInput;
        playerData = playerInteraction.playerData;
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
        SaveSystem.Initialize();
        dynamicBatcher = GetComponent<DynamicBatcher>();
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
        yield return new WaitForSeconds(20f);
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
    public async Task SpawnCarnivalTruck()
    {
        while (playerData.inUI)
        {
            await Task.Yield();
        }
        Instantiate(gameModeData.carnivalTruckPref,new Vector3(-17, -9.86675167f, -4.67279959f),new Quaternion(0, -0.707106829f, 0, 0.707106829f));
    }

    public void TakePicture()
    {
        for (int i = 1; i <=1000; i++)
        {
            if (!File.Exists(Application.persistentDataPath + "ScreenShot" + i + ".png"))
            {
                SaveSystem.SaveScreenShot(i);
                break;
            }
        }
    }

    void OnEnable() => Load(0);
    public void Save(int gameNumber)
    {
        SaveSystem.HandleSaving(gameNumber);
    }

    public void Load(int gameNumber)
    {
        SaveSystem.HandleLoading(gameNumber);
    }
    public void SpawnSavedObjects()
    {
        foreach (var r in SaveSystem.SaveGameData.respawnables)
        {
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
        SaveSystem.SaveGameData.respawnables = new List<RespawbableData>();
    }
}