using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : Interactable
{
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private ObjectHolder objType;
    private Transform obj;
    private bool full;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if(!full)
        {
            obj = Instantiate(objType.gObj, spawnTrans.position, spawnTrans.rotation).transform;
            obj.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (obj != null && other.gameObject == obj.gameObject)
        {
            full = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (obj != null && other.gameObject == obj.gameObject)
        {
            full = false;
        }
    }

    public override void OnFocus()
    {
    }
    public override void OnLoseFocus()
    {
    }
}
