using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FridgeInteractable : Interactable
{
    private Fridge fridge;
    public bool open;
    [SerializeField]private Transform openTransform;
    [SerializeField]private Transform startTransform;
    private float time = 1.5f;

    private void Update()
    {
        Debug.Log(open);
    }

    void Start()
    {
        fridge = transform.root.GetComponent<Fridge>();
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        //fridge.RotateObj(this.gameObject,open,time, startTransform,openTransform);
    }

    public override void OnFocus()
    {
        throw new NotImplementedException();
    }

    public override void OnLoseFocus()
    {
        throw new NotImplementedException();
    }
}
