using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrashRemover : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Trash"))
        {
            StartCoroutine(WaitToDestroy(other.gameObject));
        }
    }

    private IEnumerator WaitToDestroy(GameObject other)
    {
        float killTime = 1;
        float currentTime = 0;
        var rb =other.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        while (currentTime<=killTime)
        {
            if(rb)
                rb.AddForce(new Vector3(Random.Range(-1f*rb.mass,1f*rb.mass),Random.Range(-.5f*rb.mass,.5f*rb.mass),Random.Range(-1f*rb.mass,1f*rb.mass)));
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(.02f);        
        Destroy(other);
    }
}