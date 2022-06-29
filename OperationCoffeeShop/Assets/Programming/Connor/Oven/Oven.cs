using System.Collections;
using UnityEngine;

public class Oven : MachineInteraction
{
    public Animator ovenAnimator;

    private const string IsOpen = "isOpen";

    private bool _canOpen = true;
    private static readonly int Open = Animator.StringToHash(IsOpen);


    public override void Start()
    {
        base.Start();
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        StartCoroutine(CO_Interact());
    }

    private IEnumerator CO_Interact()
    {
        if (_canOpen)
        {
            _canOpen = false;
            ovenAnimator.SetBool(Open, ovenAnimator.GetBool(Open) != true);
            yield return new WaitForSeconds(1);
            _canOpen = true;
        }

        yield return null;
    }
}