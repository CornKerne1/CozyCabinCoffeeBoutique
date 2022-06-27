using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrinking : MonoBehaviour
{
    BoxCollider triggerBox;


    void Start()
    {
        triggerBox = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
