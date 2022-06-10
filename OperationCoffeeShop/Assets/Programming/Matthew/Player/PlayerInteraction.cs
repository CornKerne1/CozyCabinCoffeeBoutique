using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] public PlayerData pD;
    [SerializeField] private PlayerInput pI;

    public GameObject carriedObj;
    [SerializeField] public VolumeProfile profile;
    [SerializeField] private DepthOfField dof;
    [SerializeField] private Vector3 interactionPoint;
    [SerializeField] private LayerMask interactionLayer;
    private Interactable currentInteractable;
    Quaternion currentrotation;
    bool rotate;
    private float carryDistance;
    
    private bool blur;
    
    private MinFloatParameter dofDistanceParametar;
    private ClampedFloatParameter dofAperture;
    private ClampedFloatParameter startAperture;
    
    private void Awake()
    {
        pI = gameObject.GetComponent<PlayerInput>();
        pD = pI.pD;
        PlayerInput.InteractEvent += TryInteract;//
        PlayerInput.RotateEvent += TryRotate;//
        PlayerInput.RotateCanceledEvent += CancelRotate;
        PlayerInput.MoveObjEvent += MoveObj;
        PlayerInput.Alt_InteractEvent += Alt;
    }
    private void Start()
    {
        profile.TryGet<DepthOfField>(out dof);
        dofDistanceParametar = dof.focusDistance;
        dofAperture = dof.aperture;
        var d = dofAperture;
        startAperture = d;
        startAperture.value = 32.0f;
        dofAperture.value = startAperture.value;
        //pI.InteractEvent += TryInteract;
        currentrotation = gameObject.transform.localRotation;
        carryDistance = pD.carryDistance;
    }
    
    private void Update()
    {
        RaycastCheck();
        HandleCarrying();
        HandleRotation();
    }

    public PlayerInput GetPlayerInput()
    {
        return pI;
    }
    

    private void MoveObj(object sender, EventArgs e)
    {
        if (rotate) return;
        carryDistance = Mathf.Clamp(carryDistance + (pI.GetCurrentObjDistance() / 8), pD.carryDistance - pD.carryDistanceClamp, pD.carryDistance + pD.carryDistanceClamp);
    }

    public void RaycastCheck()
    {
        if (Physics.Raycast(Camera.main.ViewportPointToRay(interactionPoint), out RaycastHit hit, 1000000))
        {
            dofDistanceParametar.value = Mathf.Lerp(dofDistanceParametar.value, hit.distance, .5f);
            if (hit.distance <= pD.interactDistance)
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
        else if (currentInteractable)
        {
            dofDistanceParametar.value = Mathf.Lerp(dofDistanceParametar.value, 5, 1);
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
        else
        {

        }
    }

    public void Carry(GameObject obj)
    {
        if (pD.busyHands)
        {
            DropCurrentObj();
        }
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Collider>().isTrigger = true;
        carriedObj = obj;
        pD.busyHands = true;
        carryDistance = Mathf.Clamp(carryDistance - 1, pD.carryDistance - pD.carryDistanceClamp, pD.carryDistance + pD.carryDistanceClamp);
    }

    private void HandleCarrying()
    {
        if (pD.busyHands && carriedObj != null)
        {
            carriedObj.transform.position = Vector3.Lerp(carriedObj.transform.position, Camera.main.transform.position + Camera.main.transform.forward * carryDistance, Time.deltaTime * pD.smooth);
        }
    }

    public void DropCurrentObj()
    {
        if (carriedObj.TryGetComponent<IngredientContainer>(out IngredientContainer ingredientContainor))
        {

            Debug.Log("Nothing");
            if (!ingredientContainor.IsPouring() && !ingredientContainor.rotating && !ingredientContainor.pouringRotation)
            {
                currentInteractable.OnLoseFocus();
                carriedObj.GetComponent<Rigidbody>().isKinematic = false;
                carriedObj.GetComponent<Collider>().isTrigger = false;
                Debug.Log("1Nothing");
                ingredientContainor.inHand = false;
                //ingredientContainor.StopPouring();
                Quaternion rot = new Quaternion(Quaternion.identity.x + ingredientContainor.rotateOffset.x, Quaternion.identity.y + ingredientContainor.rotateOffset.y, Quaternion.identity.z + ingredientContainor.rotateOffset.z, Quaternion.identity.w);
                ingredientContainor.transform.rotation = rot;
                carriedObj = null;
                pD.busyHands = false;
            }
        }
        else if (carriedObj != null)
        {
            currentInteractable.OnLoseFocus();
            carriedObj.GetComponent<Rigidbody>().isKinematic = false;
            carriedObj.GetComponent<Collider>().isTrigger = false;
            if (currentInteractable)
            {
                currentInteractable.OnDrop();
            }
            currentInteractable = null;
            pD.busyHands = false;
            carriedObj = null;
            pD.busyHands = false;
        }
    }

    public void TryInteract(object sender, EventArgs e)
    {
        if (!pD.inUI)
        {
            if (pD.busyHands)
            {
                DropCurrentObj();
                AkSoundEngine.PostEvent("Play_InteractSound", gameObject);
            }
            else if (pD.canInteract && currentInteractable != null && Physics.Raycast(Camera.main.ViewportPointToRay(interactionPoint), out RaycastHit hit, pD.interactDistance, interactionLayer))
            {
                AkSoundEngine.PostEvent("Play_InteractSound", gameObject);
                currentInteractable.OnInteract(this);
            }
        }
    }

    public void Alt(object sender, EventArgs e)
    {
        if (carriedObj)
        {
            if (currentInteractable)
            {
                currentInteractable.OnAltInteract(this);
            }
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
            try
            {
                if (carriedObj.transform.root.GetComponentInChildren<Radio>()||!carriedObj.transform.root.GetComponentInChildren<IngredientContainer>().IsPouring()) {}
            }
            catch
            {

                if (pI.GetCurrentRotate().x > 0)
                {
                    carriedObj.transform.Rotate(pI.GetCurrentObjDistance() * pD.objRotationSpeed, 0, 0);
                }
                else if (pI.GetCurrentRotate().x < 0)
                {
                    carriedObj.transform.Rotate(0, pI.GetCurrentObjDistance() * pD.objRotationSpeed, 0);
                }
                else if (pI.GetCurrentRotate().y > 0)
                {
                    carriedObj.transform.Rotate(0, 0, pI.GetCurrentObjDistance() * pD.objRotationSpeed);
                }
                else if (pI.GetCurrentRotate().y < 0)
                {
                    var i = carriedObj.GetComponent<Interactable>();
                    if (i)
                    {
                        Quaternion rot = new Quaternion(Quaternion.identity.x + i.rotateOffset.x,
                            Quaternion.identity.y + i.rotateOffset.y, Quaternion.identity.z + i.rotateOffset.z,
                            Quaternion.identity.w);
                        carriedObj.transform.rotation =
                            Quaternion.Slerp(carriedObj.transform.rotation, rot, Time.deltaTime * 50);
                    }
                }
            }
        }
    }

     public void CameraBlur()
    {
         if (blur)
         {
             dofAperture.value = 1.0f;
             blur = true;
         }
         else
         {
             dofAperture.value = 32.0f;
             blur = false;
         }
     }
}
