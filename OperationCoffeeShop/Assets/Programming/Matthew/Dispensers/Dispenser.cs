using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : Interactable
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private ObjectHolder objType;
    private Transform obj;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (!pI.pD.busyHands)
        {
            obj = Instantiate(objType.gObj, spawnTrans.position, spawnTrans.rotation).transform;
            pI.Carry(obj.gameObject);
        }
    }
    public override void OnFocus()
    {
    }
    public override void OnLoseFocus()
    {
    }
}
