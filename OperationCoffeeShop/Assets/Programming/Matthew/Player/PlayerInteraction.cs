using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerInteraction : MonoBehaviour
{
    public PlayerData playerData;
    /*[SerializeField]*/ public PlayerInput playerInput;

    [SerializeField] private Vector3 interactionPoint;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private LayerMask dofLayer;
    private Interactable _currentInteractable;
    private bool _rotate;
    private float _carryDistance;
    public GameObject carriedObj;

    [SerializeField] public VolumeProfile profile;
    [SerializeField] private DepthOfField dof;
    private Camera _cam;
    private MinFloatParameter _dofDistanceParameter;
    private ClampedFloatParameter _dofAperture;
    private ClampedFloatParameter _startAperture;

    private bool _blur;

    private void Awake()//
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        playerData = playerInput.pD;
        _cam = GetComponentInChildren<Camera>();
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
        PlayerInput.InteractEvent += TryInteract;
        PlayerInput.RotateEvent += TryRotate;
        PlayerInput.RotateCanceledEvent += CancelRotate;
        PlayerInput.MoveObjEvent += MoveObj;
        PlayerInput.AltInteractEvent += Alt;
    }

    private void InitializeDof()
    {
        profile.TryGet(out dof);
        _dofDistanceParameter = dof.focusDistance;
        _dofAperture = dof.aperture;
        _dofDistanceParameter.value = 5f;
        var d = _dofAperture;
        _startAperture = d;
        _startAperture.value = 32.0f;
        _dofAperture.value = _startAperture.value;
        _carryDistance = playerData.carryDistance;
    }

    private void RaycastCheck()
    {
        if (playerData.busyHands)
        {
            if (!_currentInteractable && carriedObj)
                _currentInteractable = carriedObj.GetComponent<Interactable>();
        }
        else
        {
            if (!Physics.Raycast(_cam.ViewportPointToRay(interactionPoint), out RaycastHit hit, 1000000, dofLayer)) return; //
            _dofDistanceParameter.value = Mathf.Lerp(_dofDistanceParameter.value, hit.distance, .5f);
            if (hit.distance <= playerData.interactDistance)
            {
                if (hit.collider.gameObject.layer == 3 && (!_currentInteractable ||
                                                           hit.collider.gameObject.GetInstanceID() !=
                                                           _currentInteractable.gameObject.GetInstanceID()))
                {
                    RemoveCurrentInteractable();
                    hit.collider.TryGetComponent(out _currentInteractable);
                    if (_currentInteractable)
                        _currentInteractable.OnFocus();
                }
                else if (hit.collider.gameObject.layer != 3)
                    RemoveCurrentInteractable();
            }
            else if (_currentInteractable)
                RemoveCurrentInteractable();
        }
    }

    private void HandleCarrying()
    {
        if (!playerData.busyHands || !carriedObj) return;
        _currentInteractable.OnFocus();
        var camTrans = _cam.transform;
        carriedObj.transform.position = Vector3.Lerp(carriedObj.transform.position,
            camTrans.position + camTrans.forward * _carryDistance, Time.deltaTime * playerData.smooth);
    }

    private void HandleRotation()
    {
        if (!playerData.busyHands || !carriedObj || !_rotate) return;
        try
        {
            var root = carriedObj.transform.root;
            var a = root.GetComponentInChildren<Radio>();
            var b = !root.GetComponentInChildren<IngredientContainer>().IsPouring();
        }
        catch
        {
            if (playerInput.GetCurrentRotate().x > 0)
            {
                carriedObj.transform.Rotate(playerInput.GetCurrentObjDistance() * playerData.objRotationSpeed, 0, 0);
            }
            else if (playerInput.GetCurrentRotate().x < 0)
            {
                carriedObj.transform.Rotate(0, playerInput.GetCurrentObjDistance() * playerData.objRotationSpeed, 0);
            }
            else if (playerInput.GetCurrentRotate().y > 0)
            {
                carriedObj.transform.Rotate(0, 0, playerInput.GetCurrentObjDistance() * playerData.objRotationSpeed);
            }
            else if (playerInput.GetCurrentRotate().y < 0)
            {
                if (carriedObj.TryGetComponent<Interactable>(out var i))
                {
                    var rot = new Quaternion(Quaternion.identity.x + i.rotateOffset.x,
                        Quaternion.identity.y + i.rotateOffset.y, Quaternion.identity.z + i.rotateOffset.z,
                        Quaternion.identity.w);
                    carriedObj.transform.rotation =
                        Quaternion.Slerp(carriedObj.transform.rotation, rot, Time.deltaTime * 50);
                }
            }
        }
    }

    private void TryInteract(object sender, EventArgs e)
    {
        if (playerData.inUI) return;
        if (playerData.busyHands)
        {
            DropCurrentObj();
            AkSoundEngine.PostEvent("Play_InteractSound", gameObject);
        }
        else if (playerData.canInteract && _currentInteractable && Physics.Raycast(
                     _cam.ViewportPointToRay(interactionPoint),
                     playerData.interactDistance, interactionLayer))
        {
            AkSoundEngine.PostEvent("Play_InteractSound", gameObject);
            _currentInteractable.OnInteract(this);
        }
    }

    private void TryRotate(object sender, EventArgs e)
    {
        _rotate = true;
    }

    private void CancelRotate(object sender, EventArgs e)
    {
        _rotate = false;
    }

    private void MoveObj(object sender, EventArgs e)
    {
        if (_rotate) return;
        _carryDistance = Mathf.Clamp(_carryDistance + (playerInput.GetCurrentObjDistance() / 8),
            playerData.carryDistance - playerData.carryDistanceClamp,
            playerData.carryDistance + playerData.carryDistanceClamp);
    }

    private void Alt(object sender, EventArgs e)
    {
        if (!carriedObj) return;
        if (_currentInteractable)
        {
            _currentInteractable.OnAltInteract(this);
        }
    }

    private void RemoveCurrentInteractable()
    {
        if (!_currentInteractable) return;
        _currentInteractable.OnLoseFocus();
        _currentInteractable = null;
    }

    public void DropCurrentObj()
    {
        if (!carriedObj)
            return;
        if (carriedObj.TryGetComponent<IngredientContainer>(out var ingredientContainer))
        {
            if (ingredientContainer.IsPouring() || ingredientContainer.rotating ||
                ingredientContainer.pouringRotation) return;
            RemoveCurrentInteractable();
            carriedObj.GetComponent<Rigidbody>().isKinematic = false;
            carriedObj.GetComponent<Collider>().isTrigger = false;
            ingredientContainer.inHand = false;
            var rot = new Quaternion(Quaternion.identity.x + ingredientContainer.rotateOffset.x,
                Quaternion.identity.y + ingredientContainer.rotateOffset.y,
                Quaternion.identity.z + ingredientContainer.rotateOffset.z, Quaternion.identity.w);
            ingredientContainer.transform.rotation = rot;
            carriedObj = null;
            playerData.busyHands = false;
        }
        else if (carriedObj.TryGetComponent<ExamineInteractable>(out var examineInteractable))
        {
            if (_currentInteractable)
                _currentInteractable.OnLoseFocus();
            carriedObj.GetComponent<Collider>().isTrigger = false;
            _currentInteractable = null;
            playerData.busyHands = false;
            carriedObj = null;
            playerData.busyHands = false;
            examineInteractable.ReturnToOriginalPosition();
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
            playerData.busyHands = false;
            carriedObj = null;
            playerData.busyHands = false;
        }
    }

    public void Carry(GameObject obj)
    {
        if (playerData.busyHands)
        {
            DropCurrentObj();
        }

        RemoveCurrentInteractable();
        obj.TryGetComponent(out _currentInteractable);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponent<Collider>().isTrigger = true;
        carriedObj = obj;
        playerData.busyHands = true;
        _carryDistance = Mathf.Clamp(_carryDistance - 1, playerData.carryDistance - playerData.carryDistanceClamp,
            playerData.carryDistance + playerData.carryDistanceClamp);
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

    private void OnDestroy()
    {
        PlayerInput.InteractEvent -= TryInteract;
        PlayerInput.RotateEvent -= TryRotate;
        PlayerInput.RotateCanceledEvent -= CancelRotate;
        PlayerInput.MoveObjEvent -= MoveObj;
        PlayerInput.AltInteractEvent -= Alt;
    }
}