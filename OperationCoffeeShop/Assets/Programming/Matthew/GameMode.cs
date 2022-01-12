using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    //This class keeps track of the game
    
    //This is the Scriptable Object that contains the data for this class.
    public GameModeData gMD;
    //This is a component that does not inherit from monobehavior. This class calls logic within that component. 
    public DayNightCycle dNC;
    
    // Start is called before the first frame update
    void Start()
    {
        gMD.gameModeInstance = this;
        dNC = new DayNightCycle(dNC, this, gMD);
        gMD.currentOpenTime = gMD.openTimer;
    }

    // Update is called once per frame
    void Update()
    {
        dNC.StartTimer();
    }

    public void UpdateTimeOfDay(int time)
    {
        dNC.UpdateTimeOfDay(time);
    }
}
