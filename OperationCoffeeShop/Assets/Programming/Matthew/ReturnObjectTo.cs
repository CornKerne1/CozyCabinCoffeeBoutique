using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnObjectTo : MonoBehaviour
{

    [SerializeField] private Transform _target;
    [SerializeField] private Transform _otherObj;
    [SerializeField] private bool _isOn;
    [Range(0.0f, 1f)] [SerializeField] private float _interpSpeed;
    [SerializeField] private CharacterController _characterController;

    private void Start()
    {
        if (_otherObj.TryGetComponent<CharacterController>(out _characterController)) ;

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isOn) return;
        _characterController.enabled = false;
        _otherObj.position = Vector3.Lerp(_otherObj.position,_target.position,Time.deltaTime*_interpSpeed);
        
        var rotation = _otherObj.rotation;
        var rotation1 = _target.rotation;
        rotation = new Quaternion(Mathf.Lerp(rotation.x, rotation1.x, Time.deltaTime*_interpSpeed*.1f),
            Mathf.Lerp(rotation.y, rotation1.y, Time.deltaTime*_interpSpeed*.1f),Mathf.Lerp(rotation.z,
                rotation1.z, Time.deltaTime*_interpSpeed*.1f),
            Mathf.Lerp(rotation.w, rotation1.w, Time.deltaTime*_interpSpeed*.1f));
        _otherObj.rotation = rotation;
    }
}
