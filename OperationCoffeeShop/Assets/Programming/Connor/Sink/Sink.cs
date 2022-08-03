using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Sink : Machine
{
    private ObjectPool<GameObject> _hotWaterPool;
    private ObjectPool<GameObject> _coldWaterPool;

    public Animator Animator;

    public bool isHotWater;

    private new void Start()
    {
        base.Start();
        _hotWaterPool = new ObjectPool<GameObject>(
            () => { return Instantiate(iD.hotWater, outputTransform.position, outputTransform.rotation); },
            gameObject =>
            {
                gameObject.SetActive(true);
                Debug.Log("spawning hot water");
                gameObject.transform.position = outputTransform.position;
                gameObject.transform.rotation = outputTransform.rotation;
            },
            gameObject => { gameObject.SetActive(false); }, gameObject => { Destroy(gameObject); }, true, 100, 100);
        _coldWaterPool = new ObjectPool<GameObject>(
            () => { return Instantiate(iD.coldWater, outputTransform.position, outputTransform.rotation); },
            gameObject =>
            {
                gameObject.SetActive(true);
                Debug.Log("spawning cold water");

                gameObject.transform.position = outputTransform.position;
                gameObject.transform.rotation = outputTransform.rotation;
            },
            gameObject => { gameObject.SetActive(false); }, gameObject => { Destroy(gameObject); }, true, 100, 100);
    }

    protected override IEnumerator ActivateMachine(float _)
    {
        AkSoundEngine.PostEvent("PLAY_LOOPPOUR", gameObject);
        isRunning = true;
        while (isRunning)
        {
            if (isHotWater)
            {
                _hotWaterPool.Get();
            }
            else
            {
                _coldWaterPool.Get();
            }

            yield return new WaitForSeconds(.04f);
        }

        AkSoundEngine.PostEvent("STOP_LOOPPOUR", gameObject);
    }
}