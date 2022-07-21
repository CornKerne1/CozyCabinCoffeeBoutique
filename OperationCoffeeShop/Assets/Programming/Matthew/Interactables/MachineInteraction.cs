using System;
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
                Debug.Log("sdfgsdfgsdfgsdfg????" + gameObject.name);

                if (!bB.open && Machine.currentCapacity > 0)
                {
                    Debug.Log("hellowwwww????" + gameObject.name);

                    IfTutorial();
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

    private void IfTutorial()
    {
        Debug.Log("teeeeeeesssssssttttttt brewer bowl interaction");
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }
}