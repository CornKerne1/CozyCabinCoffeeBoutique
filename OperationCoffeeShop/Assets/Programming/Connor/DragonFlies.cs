using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFlies : MonoBehaviour
{
    private void Start()
    {
        AkSoundEngine.PostEvent("PLAY_DRAGONFLIES", gameObject);
    }

    private void OnDisable()
    {
        AkSoundEngine.PostEvent("STOP_DRAGONFLIES", gameObject);
    }
}