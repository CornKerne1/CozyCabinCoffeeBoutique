using System.Collections;
using UnityEngine;

public class Chest : Interactable
{
    private Animator _animator;
    private static readonly int OpenClose = Animator.StringToHash("OpenClose");

    private bool _isntOpen = true;

    public override void Start()
    {
        base.Start();
        _animator = transform.root.GetComponentInChildren<Animator>();
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        _animator.SetTrigger(OpenClose);
        _isntOpen = !_isntOpen;
        AkSoundEngine.PostEvent(_isntOpen ? "PLAY_CABINETCLOSE" : "PLAY_CABINETOPEN", gameObject);
    }
}