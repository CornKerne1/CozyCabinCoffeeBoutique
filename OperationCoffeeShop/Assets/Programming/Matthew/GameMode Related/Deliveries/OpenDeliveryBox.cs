using UnityEngine;

public class OpenDeliveryBox : Interactable
{
    public override void OnDrop()
    {

        foreach (var t in transform.GetChildren(transform))
        {
            t.GetComponent<Rigidbody>().isKinematic = false;
        }
        transform.DetachChildren();
        Destroy(gameObject);
        
    }
}