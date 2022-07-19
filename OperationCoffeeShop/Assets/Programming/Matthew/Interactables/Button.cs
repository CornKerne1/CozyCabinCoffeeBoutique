using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Button : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction _playerInteraction;

    public static event EventHandler OpenShop;


    [FormerlySerializedAs("ButtonAnimator")]
    public Animator buttonAnimator;

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        buttonAnimator.SetTrigger("Press");
        if (!gameMode.gameModeData.isOpen)
        {
            gameMode.OpenShop();
            AkSoundEngine.PostEvent("Play_buttonpress", this.gameObject);
            if (gameMode.gameModeData.isOpen)
            {
                OpenShop?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void OnAltInteract(PlayerInteraction playerInteraction)
    {
    }
}