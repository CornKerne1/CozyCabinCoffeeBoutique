using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "OptionsData", menuName = "Options/Generic")]
public class ScriptableOptions : ScriptableObject
{
    [FormerlySerializedAs("MasterVol")] public float masterVol;
    [FormerlySerializedAs("MusicVol")] public float musicVol;
    [FormerlySerializedAs("SFXVol")] public float sfxVol;

    private void OnEnable()
    {
        if (masterVol == 0)
            masterVol = .4f;
        if (musicVol == 0)
            musicVol = .4f;
        if (sfxVol == 0)
            sfxVol = .4f;
    }

    public void InitializeAudio()
    {
        AkSoundEngine.SetRTPCValue("MasterVolume", masterVol);
        AkSoundEngine.SetRTPCValue("MasterVolume", musicVol);
        AkSoundEngine.SetRTPCValue("MasterVolume", sfxVol);
    }
}