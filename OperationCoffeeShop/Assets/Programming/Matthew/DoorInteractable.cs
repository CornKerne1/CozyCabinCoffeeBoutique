using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    private Door door;
    // Start is called before the first frame update
    void Start()
    {
        door = transform.root.GetComponentInChildren<Door>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        door.PlayerOpen();
    }

    public override void OnFocus()
    {
    }

    public override void OnLoseFocus()
    {
    }

    public override void OnAltInteract(PlayerInteraction playerInteraction)
    {
       
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
