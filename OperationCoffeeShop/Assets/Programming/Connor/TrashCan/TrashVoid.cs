using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashVoid : MonoBehaviour
{
    PlayerInteraction PI;
    private void Start()
    {
        PI= GameObject.Find("Player 1").GetComponent<PlayerInteraction>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp"&& PI.carriedObj != other.gameObject)
        {
            other.gameObject.SetActive(false);
        }
    }
}
