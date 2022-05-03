using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrinderInteraction : MachineInteraction
{
    public Animator animator;
    public override void OnInteract(PlayerInteraction pI)
    {

        animator.SetTrigger("Press");
        StartCoroutine(Grind());
        
    }
    IEnumerator Grind()
    {
        yield return new WaitForSeconds(1);
        switch (mD.machineType)
        {
            case MachineData.Type.Default:
                machine.StartMachine();
                 break;
            case MachineData.Type.Brewer:
                var bB = transform.root.GetComponentInChildren<BrewerBowl>();
                if (bB)
                {
                    if (!bB.open && machine.currentCapacity > 0)
                    {
                        machine.StartMachine(); bB.filter.SetActive(false);
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
