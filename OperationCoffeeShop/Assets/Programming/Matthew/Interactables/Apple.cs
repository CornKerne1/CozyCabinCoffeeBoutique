using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Apple : Interactable
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

    private async void WaitToKill()
    {
        await Task.Delay(10);
        await KillObj();
    }

    private async Task KillObj()
    {
        Destroy(gameObject);
    }
}