using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameMode : MonoBehaviour
{
    //This class keeps track of the game

    [SerializeField]public Transform player;
    [SerializeField]public GameObject playerPref;
    [SerializeField]private Gate gate;
    //This is the Scriptable Object that contains the data for this class.
    public GameModeData gMD;
    //This is a component that does not inherit from monobehavior. This class calls logic within that component. 
    public DayNightCycle dNC;

    private List<GameObject> toBeDestroyed = new List<GameObject>();
    

    [SerializeField] public GameObject sunLight;

    [SerializeField] private GameObject GameOver;

    // Start is called before the first frame update
    private void Start()
    {
        //player.gameObject.SetActive(true);
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
        gMD.startTime = DateTime.Now.Date + TimeSpan.FromHours(6);
        gMD.currentTime = gMD.startTime;

    }

    public void DeactivateAndDestroy(GameObject obj)
    {
        obj.SetActive(false);
        toBeDestroyed.Add(obj);
    }

    public void OpenShop()
    {
        if (gMD.currentTime.Hour < 18 && gMD.currentTime.Hour > 6)
        {
            gMD.isOpen = true;
            gate.OpenCloseGate();
        }
    }
    public void CloseShop()
    {
        gMD.isOpen = false;
        gate.OpenCloseGate();
        Instantiate(GameOver);
    }
}
