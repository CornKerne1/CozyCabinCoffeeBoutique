using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    //This class keeps track of the game

    [SerializeField]private Transform player;
    //This is the Scriptable Object that contains the data for this class.
    public GameModeData gMD;
    //This is a component that does not inherit from monobehavior. This class calls logic within that component. 
    public DayNightCycle dNC;

    // Start is called before the first frame update
    private void Start()
    {
        gMD.currentTime = DateTime.Now.Date + TimeSpan.FromHours(gMD.startTime);
    }
    void Awake()
    {
        //Creates new DayNightCycle component.
        dNC = new DayNightCycle(dNC, gMD);
        Initialize();
    }
    
    //Update is called once per frame
    void Update()
    {
        //Handles the timer when the store is open.
        dNC.StartTimer();
        if(gMD.isOpen)
        {
            player.gameObject.SetActive(true);
        }
        else
        {
            player.gameObject.SetActive(false);
        }
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

    }
}
