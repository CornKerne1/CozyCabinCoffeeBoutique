using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDrawer : Interactable
{
    [SerializeField] private GameObject door;
    [SerializeField] private Transform startTrans;
    [SerializeField] private Transform openTrans;
    private bool open = false;
    private bool running;
    void Start()
    {
        var t = transform;
        startTrans = t;
    }

    public override void OnInteract(PlayerInteraction pI)
    {
        if (!running)
        {
            running = true;
        }
    }

    public override void OnFocus()
    {
    }

    public override void OnLoseFocus()
    {
    }

    void Update()
    {
        if (running)
        {
            if (!open)
            {
                door.transform.rotation = Quaternion.Lerp(door.transform.rotation, openTrans.rotation, 2.5f * Time.deltaTime);
                if (door.transform.rotation == openTrans.rotation)
                {
                    open = true;
                    running = false;
                }
            }
            else
            {
                door.transform.rotation = Quaternion.Lerp(door.transform.rotation, startTrans.rotation, 2.5f * Time.deltaTime);
                if (door.transform.rotation == startTrans.rotation)
                {
                    open = false;
                    running = false;
                }
                
            }
        }
    }
}
