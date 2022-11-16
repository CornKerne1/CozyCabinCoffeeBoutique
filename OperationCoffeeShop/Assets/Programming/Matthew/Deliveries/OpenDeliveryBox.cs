using UnityEngine;

public class OpenDeliveryBox : Interactable
{
    public override void OnDrop()
    {
        
        transform.DetachChildren();
        Destroy(gameObject);
        
    }
}