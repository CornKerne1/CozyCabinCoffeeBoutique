using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MachineData", menuName = "Machine/Generic")]
public class MachineData : ScriptableObject
{
    [SerializeField] public int maxCapacity;
    [SerializeField] public Ingredients acceptedIngredient;
    [SerializeField] public GameObject outputIngredient;
    [Range(0, 1)]
    [SerializeField] public float vibeAmt;
    [Range(0, 1000)]
    [SerializeField] public float vibeSpeed;

}
