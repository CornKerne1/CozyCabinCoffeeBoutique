using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private Task _taskRunning;
    private bool _frozen;
    async void Start()
    {
        _taskRunning = StopPhysics();
        await StopPhysics();
    }

    private async Task StopPhysics()
    {
        var childRigidBodies = transform.GetComponentsInChildren<Rigidbody>();
        var notMoving = false;
        await Task.Delay(1000);
        while (!notMoving)
        {
            foreach (var rB in childRigidBodies)
            {
                rB.gameObject.layer = LayerMask.NameToLayer("Trash");
                if (rB.velocity.magnitude < .1f)
                {
                    rB.isKinematic = true;
                }
            }
            notMoving = true;
            await Task.Yield();
        }
        _taskRunning = null;
    }

    private void OnDestroy()
    {
      
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
