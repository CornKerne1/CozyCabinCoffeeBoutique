using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public GameModeData gMD;
    
    // Start is called before the first frame update
    void Start()
    {
        gMD.gameModeInstance = this;
        gMD.currentOpenTime = gMD.openTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if(gMD.isOpen)
        {
            gMD.currentOpenTime -= 1 * Time.deltaTime;
        }
    }

    public void UpdateTimeOfDay(int time)
    {
        if(gMD.timeOfDay + time > 24)
        {
            gMD.timeOfDay = (gMD.timeOfDay + time) - 24;
        }
        gMD.timeOfDay += time;
    }
}
