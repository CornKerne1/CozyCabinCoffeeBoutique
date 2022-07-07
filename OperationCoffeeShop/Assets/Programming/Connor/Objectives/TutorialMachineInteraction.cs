using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMachineInteraction : Interactable
{
    protected Machine Machine;
    public MachineData mD;
    private Objectives1 _objectives1;

    public override void Start()
    {
        base.Start();
        Machine = transform.root.GetComponentInChildren<TutorialCoffeeBrewer>();
        _objectives1 = GameObject.Find("Objectives").GetComponent<Objectives1>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        switch (mD.machineType)
        {
            case MachineData.Type.Default:
                Machine.StartMachine();
                return;
            case MachineData.Type.Brewer:

                var bB = transform.root.GetComponentInChildren<TutorialBrewerBowl>();
                Debug.Log("is this brewer? " + bB);

                if (!bB)
                {
                    return;
                }

                if (!bB.open && Machine.currentCapacity > 0)
                {
                    Debug.Log("hellowwwww????" + gameObject.name);
                    _objectives1.NextObjective(gameObject);
                    Machine.StartMachine();
                    bB.filter.SetActive(false);
                }
                else
                {
                    Debug.Log("open says me");
                    bB.OpenOrClose();
                }

                return;
            case MachineData.Type.Grinder:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}