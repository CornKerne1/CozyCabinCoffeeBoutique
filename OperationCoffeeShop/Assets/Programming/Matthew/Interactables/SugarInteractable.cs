using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarInteractable : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction pI;
    [SerializeField] private GameObject sugarCube;

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        this.pI = playerInteraction;
        playerInteraction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction playerInteraction)
    {
        
    }

    public override void OnDrop()
    {
        Instantiate(sugarCube, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
