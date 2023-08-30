using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BounceTrigger : MonoBehaviour
{
    public float bounceForce = 10f; // Adjust the force according to your needs
    private bool _inTrigger;

    private void Start()
    {
        // Get the CharacterController component attached to the player
    }

    private async void OnTriggerEnter(Collider other)
    {
        _inTrigger = true;
        try
        {
            var cC = other.transform.parent.GetComponentInChildren<CharacterController>();
            Vector3 bounceDirection = other.transform.position - transform.position;
            bounceDirection.Normalize();
            Vector3 velocity = bounceDirection * bounceForce;
            while (_inTrigger)
            {
                cC.Move(velocity * Time.deltaTime);
                velocity -= velocity * Time.deltaTime; 
                await Task.Yield();
            }
        }
        catch
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _inTrigger = false;
    }
}

