using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioChannel: MonoBehaviour
{
    public int channel;
    public string eventName;
    public Radio radio;
    public void StartChannel()
    {
        switch (channel)
        {
            case 0:
                eventName = "Play_AfterHours";
                break;
            case 1 :
                eventName = "Play_BossaNova";
                break;
            case 2:
                eventName = "Play_BreakingLoose";
                break;
            case 3 :
                eventName = "Play_ExquisiteTaste";
                break;
            case 4:
                eventName = "Play_NeuroticBone";
                break;
            case 5 :
                eventName = "Play_RoastBlend";
                break;
            case 6:
                eventName = "Play_SomedayWakeUp";
                break;
            case  7:
                eventName = "Play_SpashedSea";
                break;
            case 8:
                eventName = "Play_TakingShape";
                break;
            case 9 :
                eventName = "Play_TellingStories";
                break;
        }
        PostSoundEvent(eventName);
        StopChannel();
    }
    public void PlayChannel()
    {
        switch (channel)
        {
            case 0:
                eventName = "VolumeOne_AfterHours";//
                break;
            case 1 :
                eventName = "VolumeOne_BossaNova";
                break;
            case 2:
                eventName = "VolumeOne_BreakingLoose";
                break;
            case 3 :
                eventName = "VolumeOne_ExquisiteTaste";
                break;
            case 4:
                eventName = "VolumeOne_NeuroticBone";
                break;
            case 5 :
                eventName = "VolumeOne_RoastBlend";
                break;
            case 6:
                eventName = "VolumeOne_SomedayWakeUp";
                break;
            case  7:
                eventName = "VolumeOne_SpashedSea";
                break;
            case 8:
                eventName = "VolumeOne_TakingShape";
                break;
            case 9 :
                eventName = "VolumeOne_TellingStories";
                break;
        }
        PostSoundEvent(eventName);
    }
    public void StopChannel()
    {
        switch (channel)
        {
            case 0:
                eventName = "VolumeZero_AfterHours";
                break;
            case 1 :
                eventName = "VolumeZero_BossaNova";
                break;
            case 2:
                eventName = "VolumeZero_BreakingLoose";
                break;
            case 3 :
                eventName = "VolumeZero_ExquisiteTaste";
                break;
            case 4:
                eventName = "VolumeZero_NeuroticBone";
                break;
            case 5 :
                eventName = "VolumeZero_RoastBlend";
                break;
            case 6:
                eventName = "VolumeZero_SomedayWakeUp";
                break;
            case  7:
                eventName = "VolumeZero_SpashedSea";
                break;
            case 8:
                eventName = "VolumeZero_TakingShape";
                break;
            case 9 :
                eventName = "VolumeZero_TellingStories";
                break;
        }
        PostSoundEvent(eventName);
    }

    public void PostSoundEvent(string s)
    {
        AkSoundEngine.PostEvent(s, this.gameObject);
    }
}
