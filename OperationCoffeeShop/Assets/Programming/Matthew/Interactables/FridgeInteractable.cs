using System.Collections;
using UnityEngine;

public class FridgeInteractable : Interactable
{
    private Animator _animator;
    private bool _opening;
    private bool _open;
    private static readonly int Close = Animator.StringToHash("Close");
    private static readonly int Open = Animator.StringToHash("Open");

    private void Update()
    {
       
    }

    public override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        if (_opening) return;
        if (_open)
        {
            AkSoundEngine.PostEvent("Play_FridgeDoorSqueak", gameObject);
            _animator.SetTrigger(Close);
            StartCoroutine(Timer(1f));
            _opening = true;
        }
        else
        {
            AkSoundEngine.PostEvent("Play_FridgeDoorSqueak", gameObject);
            AkSoundEngine.PostEvent("Play_FridgeOpen", gameObject);
            _animator.SetTrigger(Open);
            StartCoroutine(Timer(1f));
            _opening = true;
        }
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        _opening = false;
        _open = !_open;
        if (!_open)
        {
            AkSoundEngine.PostEvent("Play_FridgeClosed", gameObject);
        }
    }

}
