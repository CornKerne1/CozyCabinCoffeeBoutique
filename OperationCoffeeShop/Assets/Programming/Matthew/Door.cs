using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private Transform startTrans;

    [SerializeField] private Transform openTrans;
    
     [SerializeField] private bool playerDoor;
    
    private bool open;

    private bool running;

    private bool occupied;
    
    // Start is called before the first frame update
    void Start()
    {
        startTrans = transform;
    }

    // Update is called once per frame
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
                    StartCoroutine(TryClose());
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

    private IEnumerator TryClose()
    {
        yield return new WaitForSeconds(1f);
        if (!occupied)
        {
            running = true;
        }
        else
        {
            StartCoroutine(TryClose());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(playerDoor)
        {
            if (other.tag == "Player")
            {
                if (running && open)
                {
                    open = false;
                }
                else if (!open)
                {
                    running = true;
                }
            }
        }
        else if (other.tag == "Customer")
        {
            if (running && open)
            {
                open = false;
            }
            else if (!open)
            {
                running = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Customer")
        {
            occupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Customer")
        {
            occupied = false;
        } 
    }
}
