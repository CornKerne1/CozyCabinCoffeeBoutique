using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    private ParticleSystem particleSystem;
    private void OnTriggerEnter(Collider other)
    {
        particle.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if(!particleSystem)
            particle.GetComponent<ParticleSystem>();
        else
            particleSystem.Stop();
    }
}
