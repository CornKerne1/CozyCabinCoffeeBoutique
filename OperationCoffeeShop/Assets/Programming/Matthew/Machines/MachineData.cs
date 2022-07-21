using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineData", menuName = "Machine/Generic")]
public class MachineData : ScriptableObject
{
    
    public enum Type { Default, Brewer, Grinder }
    [SerializeField] public Type machineType = Type.Default;
    [SerializeField] public int maxCapacity;
    [SerializeField] public List<GameObject> outputIngredient = new List<GameObject>();

    [SerializeField] public Vector3 vibeAmt = new Vector3(.01f, .01f, .01f);
    [Range(0, 1000)]
    [SerializeField] public float vibeSpeed;
    
    [SerializeField] public float productionTime = 5;

    private void OnEnable()
    {
        outputIngredient = new List<GameObject>();
    }
}
