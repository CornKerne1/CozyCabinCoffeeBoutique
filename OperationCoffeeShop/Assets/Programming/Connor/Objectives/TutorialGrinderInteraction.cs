using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialGrinderInteraction : MachineInteraction
{
    public GameObject objectiveOutputObject;
    private Objectives1 _objectives1;

    private GameMode _gameMode;
    
    public Animator animator;

    public override void Start()
    {
        base.Start();
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _objectives1 = GameObject.Find("Objectives").GetComponent<Objectives1>();
    }
    public override void OnInteract(PlayerInteraction playerInteraction)
    {

        animator.SetTrigger("Press");
        StartCoroutine(Grind());
        _objectives1.NextObjective(objectiveOutputObject);

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
                var bB = transform.root.GetComponentInChildren<TutorialBrewerBowl>();
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