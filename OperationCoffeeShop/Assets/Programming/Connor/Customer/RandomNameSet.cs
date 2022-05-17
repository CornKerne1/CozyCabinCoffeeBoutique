using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Customer/RandomNameSet")]
public class RandomNameSet : ScriptableObject
{
    [SerializeField]
   public List<string> names = new List<string>();
}
