using System;
using UnityEngine;


public class MachineInteraction : Interactable
{
    protected Machine Machine;
    public MachineData mD;
    private static readonly int IsOpen = Animator.StringToHash("isOpen");

    public override void Start()
    {
        base.Start();
        Machine = transform.root.GetComponentInChildren<Machine>();
    }

    public override void OnInteract(PlayerInteraction interaction)
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
            case MachineData.Type.PlayCube:
                GetComponentInParent<PlayCube>().OnInteract(interaction);
                break;
            case MachineData.Type.Sink:
            case MachineData.Type.Grinder:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void IfTutorial()
    {
        
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }
}