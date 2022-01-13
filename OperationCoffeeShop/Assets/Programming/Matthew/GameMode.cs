using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public GameModeData gMD;
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
