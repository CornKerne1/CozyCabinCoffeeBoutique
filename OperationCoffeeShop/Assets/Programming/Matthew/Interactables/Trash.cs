using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StopPhysics());
    }

    private IEnumerator StopPhysics()
    {
        var childRigidBodies = transform.GetComponentsInChildren<Rigidbody>();
        var notMoving = false;
        yield return new WaitForSeconds(1f);
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
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
