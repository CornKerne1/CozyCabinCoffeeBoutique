using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.Serialization;

public class OpenSign : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction _playerInteraction;
    
    [SerializeField]private Animator animator;
    [SerializeField] private Transform openTrans;
    private static readonly int Open = Animator.StringToHash("Open");

    public override void Start()
    {
        base.Start();
        GameMode.ShopClosed += Close;
    }

    private void Close(object sender, EventArgs e)
    {
        animator.SetBool(Open,false);
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        if (!gameMode.gameModeData.isOpen)
        {
            gameMode.OpenShop();
            AkSoundEngine.PostEvent("Play_buttonpress", this.gameObject);
            if (gameMode.gameModeData.isOpen)
            {
                animator.SetBool(Open,true);
                gameMode.player.GetComponent<PlayerMovement>().TeleportPlayer(openTrans.position);
            }
            
        }
    }

    private void OnDestroy()
    {
        GameMode.ShopClosed -= Close;
    }
}