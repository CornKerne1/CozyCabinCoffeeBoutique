using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
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
}
