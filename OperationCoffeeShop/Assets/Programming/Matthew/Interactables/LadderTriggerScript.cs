using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LadderTriggerScript : MonoBehaviour
{
    public GameObject owner;
    private Ladder _ladder;
    private void Start()
    {
        _ladder =owner.GetComponent<Ladder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(7))
        {
            owner.GetComponent<Ladder>().canClimb = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _ladder.canClimb = false;
        if (_ladder.pI !=null &&_ladder.pI.playerData)
        {
            _ladder.pI.playerData.isClimbing = false;
        }
    }
}
