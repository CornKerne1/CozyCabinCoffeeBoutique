using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This creates a file on the disk for this to be stored in the .asset format
[CreateAssetMenu(fileName = "SlabData", menuName = "SlabData/Generic")]
//The class does not inherit from MonoBehavior, since it it a Scriptable Object
public class SlabData : ScriptableObject
{
    private float _rot;
    private float _rotModifier;
    private float _attractionModifier;
    private float _productionModifier;

    private void OnEnable()
    {
        DayNightCycle.HourChanged += IncreaseRot;
    }
    private void IncreaseRot(object sender, EventArgs e)
    {
        _rot = _rot + _rotModifier;
    }
}

