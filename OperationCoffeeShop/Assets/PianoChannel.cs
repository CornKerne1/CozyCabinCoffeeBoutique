using UnityEngine;

public class PianoChannel : MonoBehaviour
{
    public int channel; 
    public string eventName;
    public Piano piano;

    public void StartChannel()
    {
        eventName = channel switch
        {
            1 => "Play_Piano",

            0 => "Play_AfterHours",
            2 => "Play_BreakingLoose",

            _ => eventName
        };
        PostSoundEvent(eventName);
        StopChannel();
    }

    public void PlayChannel()
    {
        eventName = channel switch
        {
            1 => "VolumeOne_Piano",
            0 => "VolumeOne_AfterHours",
            2 => "VolumeOne_BreakingLoose",

            _ => eventName
        };
        PostSoundEvent(eventName);
    }

    public void StopChannel()
    {
        eventName = channel switch
        {
            1 => "VolumeZero_Piano",
            0 => "VolumeZero_AfterHours",
            2 => "VolumeZero_BreakingLoose",

            _ => eventName
        };
        PostSoundEvent(eventName);
    }

    private void PostSoundEvent(string s)
    {
        Debug.Log("piano is playing: " + s + " on gameObject: " + this.gameObject);
        AkSoundEngine.PostEvent(s, this.gameObject);
    }
}