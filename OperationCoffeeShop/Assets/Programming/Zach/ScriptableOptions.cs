using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "OptionsData", menuName = "Options/Generic")]
public class ScriptableOptions : ScriptableObject
{
    public float MasterVol;
    public float MusicVol;
    public float SFXVol;
}
