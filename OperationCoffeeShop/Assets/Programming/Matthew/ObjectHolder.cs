using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//This creates a file on the disk for this to be stored in the .asset format
[CreateAssetMenu(fileName = "ObjectHolder", menuName = "ObjectHolder/GameObjects")]
//The class does not inherit from MonoBehavior, since it it a Scriptable Object
public class ObjectHolder : ScriptableObject
{
    [FormerlySerializedAs("GameObject")] [SerializeField]
    public GameObject gameObject;

    [FormerlySerializedAs("DispenserType")] [SerializeField] public DispenserType dispenserType;
}

public enum DispenserType
{
    Cup,
    Filter,
    Coffee,
    Espresso,
    Sugar,
    Tea
}