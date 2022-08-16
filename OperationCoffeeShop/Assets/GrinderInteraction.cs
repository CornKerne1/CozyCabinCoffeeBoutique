using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrinderInteraction : MachineInteraction
{
    
    public Animator animator;
    [SerializeField,Header("Tutorial Only")]    
    public GameObject objectiveOutputObject;

    public override void OnInteract(PlayerInteraction interaction)
    {

        animator.SetTrigger("Press");
        StartCoroutine(Grind());
        IfTutorial();
        
    }

    private void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(objectiveOutputObject);
        }
    }
    IEnumerator Grind()
    {
        yield return new WaitForSeconds(.4f);
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
