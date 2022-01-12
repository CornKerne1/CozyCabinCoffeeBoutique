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
        //Sets the reference to this script in the GameModeData instance.
        gMD.gameModeInstance = this;
        //Creates new DayNightCycle component.
        dNC = new DayNightCycle(dNC, this, gMD);
        //Resets old timer data.
        gMD.currentOpenTime = gMD.openTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //Handles the timer when the store is open.
        dNC.StartTimer();
    }

    //This is the method to call to change the time of day.
    public void UpdateTimeOfDay(int time)
    {
        dNC.UpdateTimeOfDay(time);
    }
}
