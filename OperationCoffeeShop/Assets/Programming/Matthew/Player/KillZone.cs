using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private GameMode _gM;
    [SerializeField]private Transform resetTrans;
    private void Start()
    {
        _gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            _gM.player.GetComponent<PlayerMovement>().TeleportPlayer(resetTrans,false);
            
            
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            _gM.player.transform.position = Vector3.zero;
        }
    }
}
