using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineInteraction : Interactable
{
    protected Machine Machine;
    public MachineData mD;

    public override void Start()
    {
        base.Start();
        Machine = transform.root.GetComponentInChildren<Machine>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        switch (mD.machineType)
        {
            case MachineData.Type.Default:
                Machine.StartMachine();
                return;
            case MachineData.Type.Brewer:
                var bB = transform.root.GetComponentInChildren<BrewerBowl>();
                if (!bB) return;
                if (!bB.open && Machine.currentCapacity > 0)
                {
                    Machine.StartMachine();
                    bB.filter.SetActive(false);
                }
                else
                {
                    bB.OpenOrClose();
                }

                return;
            case MachineData.Type.Grinder:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}