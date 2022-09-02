using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class Sink : Machine
{
    private ObjectPool<LiquidIngredients> _hotWaterPool;
    private ObjectPool<LiquidIngredients> _coldWaterPool;

    [FormerlySerializedAs("Animator")] public Animator animator;

    public bool isHotWater;

    private new void Start()
    {
        base.Start();
        _hotWaterPool = new ObjectPool<LiquidIngredients>(
            () => Instantiate(iD.hotWater.GetComponentInChildren<LiquidIngredients>(), outputTransform.position,
                outputTransform.rotation),
            ingredient =>
            {
                gameObject.SetActive(true);
                //Debug.Log("spawning hot water");
                var o = ingredient.gameObject;
                o.transform.position = outputTransform.position;
                o.transform.rotation = outputTransform.rotation;
            },
            ingredient => { ingredient.gameObject.SetActive(false); }, Destroy, true, 100, 100);
        _coldWaterPool = new ObjectPool<LiquidIngredients>(
            () => Instantiate(iD.coldWater.GetComponentInChildren<LiquidIngredients>(), outputTransform.position,
                outputTransform.rotation),
            ingredient =>
            {
                GameObject o;
                (o = ingredient.gameObject).SetActive(true);
                //Debug.Log("spawning cold water");

                o.transform.position = outputTransform.position;
                o.transform.rotation = outputTransform.rotation;
            },
            ingredient => { ingredient.gameObject.SetActive(false); }, Destroy, true, 100, 100);
    }

    protected override IEnumerator ActivateMachine(float _)
    {
        AkSoundEngine.PostEvent("PLAY_FAUCET", gameObject);
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

            yield return new WaitForSeconds(.05f);
        }

        AkSoundEngine.PostEvent("STOP_FAUCET", gameObject);
    }
}