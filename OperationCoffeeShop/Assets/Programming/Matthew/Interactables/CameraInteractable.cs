using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteractable : Interactable
{
    private Camera _camera;

    // Start is called before the first frame update

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
        lookAtPlayer = true;
        LookAtPlayer();
    }
    public override void OnAltInteract(PlayerInteraction interaction)
    {
        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
        interaction.playerInput.FreeCam();
    }

    public override void OnDrop()
    {
        base.OnDrop();
        lookAtPlayer = false;
    }
}