using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private Collider _leverCollider;

    private Animator _trashAnimator;
    private static readonly int Open = Animator.StringToHash("Open");

    private void Start()
    {
        this._trashAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (this._trashAnimator.GetBool(Open))
        {
        }
        else
        {
            this._trashAnimator.SetBool(Open, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!this._trashAnimator.GetBool(Open))
        {
        }
        else
        {
            this._trashAnimator.SetBool(Open, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (this._trashAnimator.GetBool(Open))
        {
        }
        else
        {
            this._trashAnimator.SetBool(Open, true);
        }
    }
}