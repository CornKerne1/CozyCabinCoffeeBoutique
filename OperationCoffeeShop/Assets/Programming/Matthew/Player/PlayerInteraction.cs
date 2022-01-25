using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerData pD;

    private void Start()
    {
        PlayerInput.InteractEvent += TryInteract;
    }

    private void TryInteract(object sender, EventArgs e)
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pD.interactDistance, LayerMask.NameToLayer("Interactable")))
        {
            hit.transform.gameObject.GetComponent<Interactable>().Interact();
        }
    }


}
