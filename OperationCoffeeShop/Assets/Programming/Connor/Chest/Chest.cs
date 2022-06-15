using UnityEngine;

public class Chest : Interactable
{
    private Animator _animator;
    private static readonly int OpenClose = Animator.StringToHash("OpenClose");

    public override void Start()
    {
        base.Start();
        _animator = transform.root.GetComponentInChildren<Animator>();
    }
    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        _animator.SetTrigger(OpenClose);
    }
}
