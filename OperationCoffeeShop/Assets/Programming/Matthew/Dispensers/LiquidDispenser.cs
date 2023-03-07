using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class LiquidDispenser : Dispenser
{
    private Animator _animator;
    [SerializeField] private Transform contentVisualizer;
    public override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public override void TakeFromDispenser()
    {
        if (playerInteraction.playerData.busyHands) return;
        quantity--;
        _animator.SetTrigger("Press");
        UpdateQuantity();
        UpdateVisuals();
        var ingredient = Pool.Get().transform;
        var transform1 = ingredient.transform;
        transform1.position = spawnTrans.position;
        transform1.rotation = spawnTrans.rotation;
        if (quantity == 0)
            Destroy(gameObject);
    }

    private void UpdateVisuals()
    {
        contentVisualizer.localPosition = new Vector3(0,0,((quantity * .00016f)-.016f)-0.00016f);
        contentVisualizer.localScale = new Vector3(1, 1, (quantity / 100f));
    }
}