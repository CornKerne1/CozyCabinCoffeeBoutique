using System;
using System.Collections;
using System.Drawing.Drawing2D;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class LiquidDispenser : Dispenser
{
    private Animator _animator;
    [SerializeField] private Transform contentVisualizer;
    [SerializeField]private float absoluteMaxZPos = .016f;
    [SerializeField] private int maxQuantity = 100;
    private Vector3 _newPos;
    private Vector3 _newScale;
    private bool _updating;
    
    public override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_updating)
        {
            if (Math.Abs(contentVisualizer.localPosition.z - _newPos.z) < .00001)
            {
                _newPos = Vector3.zero;
                _newScale = Vector3.zero;
                _updating = false;
                return;
            }
            contentVisualizer.localPosition = Vector3.Lerp(contentVisualizer.localPosition, _newPos, Time.deltaTime);
            contentVisualizer.localScale = Vector3.Lerp(contentVisualizer.localScale, _newScale, Time.deltaTime*3);
        }
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
        var increment = absoluteMaxZPos / maxQuantity;
        _newPos = new Vector3(0, 0, ((quantity * increment) - absoluteMaxZPos) - increment);
        _newScale = contentVisualizer.localScale = new Vector3(1, 1, (quantity / (float)maxQuantity));
        _updating = true;
    }
}