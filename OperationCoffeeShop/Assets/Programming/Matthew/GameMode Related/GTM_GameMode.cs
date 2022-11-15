using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GTM_GameMode : GameMode
{
    private new void Start()
    {
        gameModeData.timeRate = defaultTimeRate; 
    }

    protected override void Initialize()
    {
        base.Initialize();
        AkSoundEngine.PostEvent("PLAY_GRANDTHEFT_MACHIATO", player.gameObject);
    }
}