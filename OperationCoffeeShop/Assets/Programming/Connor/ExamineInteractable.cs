using UnityEngine;

public class ExamineInteractable : RandomInteractable
{
    [SerializeField] private Transform originalPosition;
    private Vector3 _orignialVector3;
    private Quaternion _originalQuaternion;


    public override void Start()
    {
        base.Start();
        _orignialVector3 = originalPosition.position;
        _originalQuaternion = originalPosition.rotation;
    }

    public void ReturnToOriginalPosition()
    {
        var transform1 = transform;
        transform1.position = _orignialVector3;
        transform1.rotation = _originalQuaternion;
    }
}