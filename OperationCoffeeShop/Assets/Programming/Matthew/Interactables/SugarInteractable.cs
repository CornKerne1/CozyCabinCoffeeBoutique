using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarInteractable : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction pI;
    [SerializeField] private GameObject sugarCube;

    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
        pI.Carry(gameObject);
    }

    public override void OnFocus()
    {

    }

    public override void OnLoseFocus()
    {
        
    }

    public override void OnAltInteract(PlayerInteraction pI)
    {
        
    }

    public override void OnDrop()
    {
        Instantiate(sugarCube, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
