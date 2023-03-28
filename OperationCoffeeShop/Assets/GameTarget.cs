using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameTarget : Interactable
{
    public override void OnInteract(PlayerInteraction interaction)
    {
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        
    }

    public bool IsBroken()
    {
        return _isBroken;
    }

    protected override async Task FreezeForClippingAsync()
    {
        if (!isBreakable) return;
        _rB.isKinematic = true;
        await Task.Delay(10);
        AkSoundEngine.PostEvent(breakableSoundEngineEvent, gameObject);
        _breakableRef = Instantiate(breakablePrefab, transform.position, transform.rotation);
        await gameMode.dynamicBatcher.AddForBatching(_breakableRef);
        _isBroken = true;
        var particle = Instantiate(gameMode.gameModeData.breakParticle, transform.position, transform.rotation);
        particle.GetComponent<ParticleSystemRenderer>().material.color = smashColor;
        particle.GetComponent<ParticleSystemRenderer>().material.SetColor("_EmissionColor", smashEmissionColor);
        foreach (var obj in _breakableRef.GetChildren(transform))
        {
            obj.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f);
        }

        float timeElapsed = 0;
        gameMode.Surprise(gameObject);
        var colliders = GetComponentsInChildren<Collider>();
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var c in colliders)
            c.enabled = false;
        foreach (var rE in renderers)
            rE.enabled = false;
        await Task.Delay(20);
        while (timeElapsed < 5f)
        {
            timeElapsed += Time.deltaTime;
            await Task.Yield();
        }
        DestroyImmediate(particle);
    }
}
