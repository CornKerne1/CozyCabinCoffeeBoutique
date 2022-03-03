using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerData pD;
    [SerializeField] private PlayerInput pI;

    [SerializeField] public DialogDisplay dD;

    GameObject carriedObj;
    [SerializeField] private Vector3 interactionPoint;
    [SerializeField] private LayerMask interactionLayer;
    private Interactable currentInteractable;
    Quaternion currentrotation;
    bool rotate;
    private void Awake()
    {
        pI = gameObject.GetComponent<PlayerInput>();
        pD = pI.pD;
        dD = gameObject.GetComponent<DialogDisplay>();
        PlayerInput.InteractEvent += TryInteract;//
        PlayerInput.RotateEvent += TryRotate;//
        PlayerInput.RotateCanceledEvent += CancelRotate;
    }
    private void Update()
    {
        RaycastCheck();
        HandleCarrying();
        HandleRotation();
    }
    private void Start()
    {
        //pI.InteractEvent += TryInteract;
        currentrotation = gameObject.transform.localRotation;
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
        else
        {

        }
    }

    public void Carry(GameObject obj)
    {
        if(pD.busyHands)
        {
            DropCurrentObj();
        }
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Collider>().isTrigger = true;
        carriedObj = obj;
        pD.busyHands = true;
    }

    private void HandleCarrying()
    {
        if (pD.busyHands && carriedObj != null)
        {
            carriedObj.transform.position = Vector3.Lerp(carriedObj.transform.position, Camera.main.transform.position + Camera.main.transform.forward * pD.carryDistance, Time.deltaTime * pD.smooth);
        }
    }

    public void DropCurrentObj()
    {
            if(carriedObj != null)
            {
                carriedObj.GetComponent<Rigidbody>().isKinematic = false;
                carriedObj.GetComponent<Collider>().isTrigger = false;
            }
            
            carriedObj = null;
            pD.busyHands = false;
    }
    
    public void TryInteract(object sender, EventArgs e)
    {
        if (pD.busyHands)
        {
            DropCurrentObj();
        }
        else if (pD.canInteract && currentInteractable != null && Physics.Raycast(Camera.main.ViewportPointToRay(interactionPoint), out RaycastHit hit, pD.interactDistance, interactionLayer))
        {
            currentInteractable.OnInteract(this);
        }

    }
    
    public void TryRotate(object sender, EventArgs e)
    {
        rotate = true;
    }


    public void CancelRotate(object sender, EventArgs e)
    {
        rotate = false;
    }
    public void HandleRotation()
    {
        if (pD.busyHands && carriedObj != null && rotate)
        {
            
             carriedObj.transform.Rotate(0,pI.GetCurrentRotate() * pD.objRotationSpeed, 0);//
                      
        }
    }
}
