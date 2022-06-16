using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Customer/RandomCustomerSet")]
public class RandomCustomerSet : ScriptableObject
{
    [SerializeField]
    public List<GameObject> customers = new List<GameObject>();
   
}
