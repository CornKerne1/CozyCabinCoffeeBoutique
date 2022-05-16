using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This creates a file on the disk for this to be stored in the .asset format
[CreateAssetMenu(fileName = "ObjectHolder", menuName = "ObjectHolder/GameObjects")]
//The class does not inherit from MonoBehavior, since it it a Scriptable Object
public class ObjectHolder : ScriptableObject
{
    [SerializeField] public GameObject gObj;
}
