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
    private Interactable _currentInteractable;
    bool _rotate;
    private float _carryDistance;
    
    private bool _blur;

    private Camera _cam;
    private MinFloatParameter _dofDistanceParameter;
    private ClampedFloatParameter _dofAperture;
    private ClampedFloatParameter _startAperture;
    
    private void Awake()
    {
        pI = gameObject.GetComponent<PlayerInput>(); pD = pI.pD; _cam = GetComponentInChildren<Camera>();
        EventSubscriber();
    }
    private void Start()
    {
        InitializeDof();
    }
    private void Update()
    {
        RaycastCheck();
        HandleCarrying();
        HandleRotation();
    }
    private void EventSubscriber()
    {
        PlayerInput.InteractEvent += TryInteract; //
        PlayerInput.RotateEvent += TryRotate; //
        PlayerInput.RotateCanceledEvent += CancelRotate;
        PlayerInput.MoveObjEvent += MoveObj;
        PlayerInput.Alt_InteractEvent += Alt;
    }
    private void InitializeDof()
    {
        profile.TryGet<DepthOfField>(out dof);
        _dofDistanceParameter = dof.focusDistance;
        _dofAperture = dof.aperture;
        var d = _dofAperture;
        _startAperture = d;
        _startAperture.value = 32.0f;
        _dofAperture.value = _startAperture.value;
        _carryDistance = pD.carryDistance;
    }
    private void MoveObj(object sender, EventArgs e)
    {
        if (_rotate) return;
        _carryDistance = Mathf.Clamp(_carryDistance + (pI.GetCurrentObjDistance() / 8), pD.carryDistance - pD.carryDistanceClamp, pD.carryDistance + pD.carryDistanceClamp);
    }

    private void RaycastCheck()
    {
        if (!pD.busyHands && Physics.Raycast(_cam.ViewportPointToRay(interactionPoint), out RaycastHit hit, 1000000))
        {
            _dofDistanceParameter.value = Mathf.Lerp(_dofDistanceParameter.value, hit.distance, .5f);
            if (hit.distance <= pD.interactDistance)
            {
                if (hit.collider.gameObject.layer == 3 && (_currentInteractable == null || hit.collider.gameObject.GetInstanceID() != _currentInteractable.GetInstanceID()))
                {
                    hit.collider.TryGetComponent(out _currentInteractable);
                    if (_currentInteractable)
                        _currentInteractable.OnFocus();
                }
                else if (_currentInteractable)
                {
                    _currentInteractable.OnLoseFocus();
                    _currentInteractable = null;
                }

            }
            else if (_currentInteractable)
            {
                _currentInteractable.OnLoseFocus();
                _currentInteractable = null;
            }

        }
        else if (_currentInteractable)
        {
            _dofDistanceParameter.value = Mathf.Lerp(_dofDistanceParameter.value, 5, 1);
            _currentInteractable.OnLoseFocus();
            _currentInteractable = null;
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
        _currentInteractable = carriedObj.GetComponent<Interactable>();
        pD.busyHands = true;
        _carryDistance = Mathf.Clamp(_carryDistance - 1, pD.carryDistance - pD.carryDistanceClamp, pD.carryDistance + pD.carryDistanceClamp);
    }

    private void HandleCarrying()
    {
        if (pD.busyHands && carriedObj != null)
        {
            carriedObj.transform.position = Vector3.Lerp(carriedObj.transform.position, Camera.main.transform.position + Camera.main.transform.forward * _carryDistance, Time.deltaTime * pD.smooth);
        }
    }

    public void DropCurrentObj()
    {
        if (carriedObj.TryGetComponent<IngredientContainer>(out IngredientContainer ingredientContainor))
        {

            Debug.Log("Nothing");
            if (!ingredientContainor.IsPouring() && !ingredientContainor.rotating && !ingredientContainor.pouringRotation)
            {
                if (_currentInteractable)
                    _currentInteractable.OnLoseFocus();
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
            if (_currentInteractable)
                _currentInteractable.OnLoseFocus();
            carriedObj.GetComponent<Rigidbody>().isKinematic = false;
            carriedObj.GetComponent<Collider>().isTrigger = false;
            if (_currentInteractable)
            {
                _currentInteractable.OnDrop();
            }
            _currentInteractable = null;
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
            else if (pD.canInteract && _currentInteractable != null && Physics.Raycast(Camera.main.ViewportPointToRay(interactionPoint), out RaycastHit hit, pD.interactDistance, interactionLayer))
            {
                AkSoundEngine.PostEvent("Play_InteractSound", gameObject);
                _currentInteractable.OnInteract(this);
            }
        }
    }

    public void Alt(object sender, EventArgs e)
    {
        if (carriedObj)
        {
            if (_currentInteractable)
            {
                _currentInteractable.OnAltInteract(this);
            }
        }
    }

    public void TryRotate(object sender, EventArgs e)
    {
        _rotate = true;
    }


    public void CancelRotate(object sender, EventArgs e)
    {
        _rotate = false;
    }
    public void HandleRotation()
    {
        if (pD.busyHands && carriedObj != null && _rotate)
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
         if (_blur)
         {
             _dofAperture.value = 1.0f;
             _blur = true;
         }
         else
         {
             _dofAperture.value = 32.0f;
             _blur = false;
         }
    }
}
