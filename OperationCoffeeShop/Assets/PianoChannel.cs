using UnityEngine;

public class PianoChannel : MonoBehaviour
{
    public int channel; //
    public string eventName;
    public Piano piano;

    public void StartChannel()
    {
        eventName = channel switch
        {
            0 => "Play_Piano",
            1 => "Play_AfterHours",
            _ => eventName
        };
        PostSoundEvent(eventName);
        StopChannel();
    }

    public void PlayChannel()
    {
        eventName = channel switch
        {
            0 => "VolumeOne_Piano",
            1 => "VolumeOne_AfterHours",
            _ => eventName
        };
        PostSoundEvent(eventName);
    }

    public void StopChannel()
    {
        eventName = channel switch
        {
            0 => "VolumeZero_Piano",
            1 => "VolumeZero_AfterHours",
            _ => eventName
        };
        PostSoundEvent(eventName);
    }

    private void PostSoundEvent(string s)
    {
        AkSoundEngine.PostEvent(s, gameObject);
    }
}