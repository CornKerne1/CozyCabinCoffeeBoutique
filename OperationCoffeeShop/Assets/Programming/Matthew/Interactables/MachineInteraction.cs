using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInteraction : Interactable
{
    protected Machine machine;
    public MachineData mD;

    public override void Start()
    {
        base.Start();
        machine = transform.root.GetComponentInChildren<Machine>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        switch (mD.machineType)
        {
            case MachineData.Type.Default:
                machine.StartMachine();
                return;
            case MachineData.Type.Brewer:
                var bB = transform.root.GetComponentInChildren<BrewerBowl>();
                if (bB)
                {
                    if (!bB.open && machine.currentCapacity > 0)
                    {
                        machine.StartMachine();
                        bB.filter.SetActive(false);
                    }
                    else
                    {
                        bB.OpenOrClose();
                    }
                }

                return;
        }
    }
}