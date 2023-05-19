using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashRemover : MonoBehaviour
{
    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Trash"))
        {
            await WaitToDestroy(other.gameObject);
        }
    }

    private async Task WaitToDestroy(GameObject other)
    {
        float killTime = 1;
        float currentTime = 0;
        var rb =other.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        while (currentTime<=killTime)
        {
            if(rb)
                rb.AddForce(new Vector3(Random.Range(-2f*rb.mass,2f*rb.mass),Random.Range(-.5f*rb.mass,.5f*rb.mass),Random.Range(-2f*rb.mass,2f*rb.mass)));
            currentTime += Time.deltaTime;
            await Task.Yield();
        }
        await Task.Delay(20);
        Destroy(other);
    }
}