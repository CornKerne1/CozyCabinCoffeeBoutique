using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashRemover : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    private ParticleSystem _particleSystem;
    private ParticleSystem.MainModule _mainModule;
    private Task _killParticleTask;

    private void Start()
    {
        if (particle)
        {
            _particleSystem = particle.GetComponent<ParticleSystem>();
            particle.SetActive(false);
        }
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Trash"))
        {
            await WaitToDestroy(other.gameObject);
        }
    }

    private async Task WaitToDestroy(GameObject other)
    {
        float killTime = 1;
        float currentTime = 0;
        var rb = other.GetComponent<Rigidbody>();
        if (particle)
        {
            if(!particle.activeSelf)
                particle.SetActive(true);
            _particleSystem.Play();
        }
        if (rb)
        {
            rb.isKinematic = false;
            while (currentTime<=killTime)
            {
                if(rb)
                    rb.AddForce(new Vector3(Random.Range(-2f*rb.mass,2f*rb.mass),Random.Range(-.5f*rb.mass,.5f*rb.mass),Random.Range(-2f*rb.mass,2f*rb.mass)));
                currentTime += Time.deltaTime;
                await Task.Yield();
            }
        }
        currentTime = 0;
        await Task.Delay(20);
        Destroy(other);
        if (particle)
        {
            if (_killParticleTask == null)
            {
                _killParticleTask = KillParticle();
                await _killParticleTask;
            }
        }
    }

    private async Task KillParticle()
    {
        while (_particleSystem.isEmitting)
        {
            await Task.Yield();
        }
        _particleSystem.Stop();
        await Task.Delay(500);
        if(!_particleSystem.isEmitting)
            _killParticleTask = null;
        else
        {
            _killParticleTask = null;
            _killParticleTask = KillParticle();
            await _killParticleTask;
        }
    }
}