using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushDelivery : MonoBehaviour
{
    [SerializeField] private GameObject SmashBush;

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "PBDeliveryTruck")
        {
            SmashBush.SetActive(false);
        }
    }
}
