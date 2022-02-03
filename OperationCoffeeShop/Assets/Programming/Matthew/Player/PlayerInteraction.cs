using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerData pD;
    [SerializeField] private PlayerInput pI;

    [SerializeField] private Vector3 interactionPoint;
    [SerializeField] private LayerMask interactionLayer;
    private Interactable currentInteractable;

    private void Awake()
    {
        pI = gameObject.GetComponent<PlayerInput>();
        pD = pI.pD;
        PlayerInput.InteractEvent += TryInteract;//
    }
    private void Update()
    {
        RaycastCheck();
    }
    private void Start()
    {
        //pI.InteractEvent += TryInteract;
    }

    public void RaycastCheck()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(interactionPoint), out RaycastHit hit, pD.interactDistance))
        {
            if (hit.collider.gameObject.layer == 3 && (currentInteractable == null || hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);
                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    public void TryInteract(object sender, EventArgs e)
    {
        if(pD.canInteract && currentInteractable != null && Physics.Raycast(Camera.main.ViewportPointToRay(interactionPoint), out RaycastHit hit, pD.interactDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
        
    }


}
