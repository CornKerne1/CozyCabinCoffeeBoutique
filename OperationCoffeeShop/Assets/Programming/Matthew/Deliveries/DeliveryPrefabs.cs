using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This creates a file on the disk for this to be stored in the .asset format
[CreateAssetMenu(fileName = "DeliveryPrefabs", menuName = "DeliveryData/DeliveryPrefabs/Generic")]
//The class does not inherit from MonoBehavior, since it it a Scriptable Object
public class DeliveryPrefabs : ScriptableObject
{
    public GameObject coffeeDispenserPrefab,espressoDispenserPrefab,sugarDispenserPrefab,milkDispenserPrefab,cameraPrefab,pictureFramePrefab,truckPrefab,openBoxPrefab;
}

