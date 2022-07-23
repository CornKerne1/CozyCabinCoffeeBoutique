using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LadderTriggerScript : MonoBehaviour
{
    public GameObject owner;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(2))
        {
            owner.GetComponent<Ladder>().canClimb = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        Ladder ladder = owner.GetComponent<Ladder>();
        if (other.gameObject.layer.Equals(2))
        {
            ladder.canClimb = false;
            if (ladder.pI !=null &&ladder.pI.playerData)
            {
                ladder.pI.playerData.isClimbing = false;
            }
        }
    }
}
