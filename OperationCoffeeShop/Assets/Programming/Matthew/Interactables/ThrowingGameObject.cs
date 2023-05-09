using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ThrowingGameObject : Interactable
{
    // Start is called before the first frame update


    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        interaction.Throw();
        WaitToKill();
    }

    private async Task WaitToKill()
    {
        await Task.Delay(5000);
        if (playerInteraction)
        {
            await WaitToKill();
            return;
        }
        await KillObj();
    }

    private async Task KillObj()
    {
        Destroy(gameObject);
    }
}