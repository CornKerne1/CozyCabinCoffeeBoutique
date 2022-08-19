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
        eventName = channel switch
        {
            0 => "Play_TellingStories",
            1 => "Play_BossaNova",
            2 => "Play_BreakingLoose",
            3 => "Play_ExquisiteTaste",
            4 => "Play_NeuroticBone",
            5 => "Play_RoastBlend",
            6 => "Play_SomedayWakeUp",
            7 => "Play_SpashedSea",
            8 => "Play_TakingShape",
            9 => "Play_RedPandaTheme",
            _ => eventName
        };
        PostSoundEvent(eventName);
        StopChannel();
    }
    public void PlayChannel()
    {
        eventName = channel switch
        {
            0 => "VolumeOne_TellingStories" //
            ,
            1 => "VolumeOne_BossaNova",
            2 => "VolumeOne_BreakingLoose",
            3 => "VolumeOne_ExquisiteTaste",
            4 => "VolumeOne_NeuroticBone",
            5 => "VolumeOne_RoastBlend",
            6 => "VolumeOne_SomedayWakeUp",
            7 => "VolumeOne_SpashedSea",
            8 => "VolumeOne_TakingShape",
            9 => "VolumeOne_RedPandaTheme",
            _ => eventName
        };
        PostSoundEvent(eventName);
    }
    public void StopChannel()
    {
        eventName = channel switch
        {
            0 => "VolumeZero_TellingStories",
            1 => "VolumeZero_BossaNova",
            2 => "VolumeZero_BreakingLoose",
            3 => "VolumeZero_ExquisiteTaste",
            4 => "VolumeZero_NeuroticBone",
            5 => "VolumeZero_RoastBlend",
            6 => "VolumeZero_SomedayWakeUp",
            7 => "VolumeZero_SpashedSea",
            8 => "VolumeZero_TakingShape",
            9 => "VolumeZero_RedPandaTheme",
 
            _ => eventName
        };
        PostSoundEvent(eventName);
    }

    public void PostSoundEvent(string s)
    {
        AkSoundEngine.PostEvent(s, this.gameObject);
    }
}
