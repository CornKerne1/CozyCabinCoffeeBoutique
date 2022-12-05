using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.Serialization;

public class OpenSign : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction _playerInteraction;
    
    private Animator _animator;
    [SerializeField] private Transform openTrans;
    private static readonly int Open = Animator.StringToHash("Open");

    public override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        if (!gameMode.gameModeData.isOpen)
        {
            gameMode.OpenShop();
            AkSoundEngine.PostEvent("Play_buttonpress", this.gameObject);
            if (gameMode.gameModeData.isOpen)
            {
                _animator.SetBool(Open,true);
                gameMode.player.GetComponent<CharacterController>().enabled = false;
                gameMode.player.transform.position = openTrans.position;
                gameMode.player.GetComponent<CharacterController>().enabled = true;
            }
            
        }
    }
    public override void OnAltInteract(PlayerInteraction interaction)
    {
    }
}