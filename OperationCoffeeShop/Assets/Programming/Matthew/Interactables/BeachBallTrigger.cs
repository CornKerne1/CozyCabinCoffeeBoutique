using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallTrigger : MonoBehaviour
{
    private Rigidbody _parentRB;

    private void Start()
    {
        _parentRB = transform.parent.gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliding");
        if (other.gameObject.layer == 7)
        {
            _parentRB.AddForce(other.transform.forward*100);
        }
    }
}
