using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> particles=new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        foreach (var obj in particles)
        {
            var pS =obj.GetComponent<ParticleSystem>();
            pS.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var obj in particles)
        {
            var pS =obj.GetComponent<ParticleSystem>();
            pS.Stop();
        }
    }
}
