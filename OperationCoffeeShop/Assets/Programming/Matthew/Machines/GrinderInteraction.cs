using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GrinderInteraction : MachineInteraction
{
    
    public Animator animator;

    public override async void OnInteract(PlayerInteraction interaction)
    {
        animator.SetTrigger("Press");
       await Grind();
        IfTutorial();
    }

    private void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }
    private async Task Grind()
    {
        await Task.Delay(400);
        switch (mD.machineType)
        {
            case MachineData.Type.Default:
                if(!Machine.isRunning)
                    Machine.PostSoundEvent("Play_GrindingCoffee");
                
                Machine.StartMachine();
                break;
            case MachineData.Type.Brewer:
                var bB = transform.root.GetComponentInChildren<BrewerBowl>();
                if (bB)
                {
                    if (!bB.open && Machine.currentCapacity > 0)
                    {
                        Machine.StartMachine(); bB.filter.SetActive(false);
                    }
                    else
                    {
                        bB.OpenOrClose();
                    }
                }
                break;

        }
    }


}
