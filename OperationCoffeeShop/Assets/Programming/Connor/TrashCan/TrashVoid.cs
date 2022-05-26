using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashVoid : MonoBehaviour
{
    GameMode gM;
    PlayerInteraction PI;
    private void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        PI = gM.player.gameObject.GetComponent<PlayerInteraction>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp"&& PI.carriedObj != other.gameObject)
        {
            other.gameObject.SetActive(false);
        }
    }
}
