using UnityEngine;

public class OpenDeliveryBox : Interactable
{
    public override void OnInteract(PlayerInteraction interaction)
    {
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        transform.DetachChildren();
        interaction.DropCurrentObj();
        Destroy(gameObject);
    }
}