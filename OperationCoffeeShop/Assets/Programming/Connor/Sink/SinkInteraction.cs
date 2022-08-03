using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkInteraction : MachineInteraction
{
    [SerializeField] private bool hotWater;
    private static readonly int HotOpen = Animator.StringToHash("Hot Open");
    private static readonly int ColdOpen = Animator.StringToHash("Cold Open");


    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        var sink = (Sink)Machine;
        if (hotWater)
        {
            if (!Machine.isRunning)
            {
                sink.isHotWater = true;
                sink.StartMachine();
            }
            else
            {
                if (sink.Animator.GetBool(HotOpen) && !sink.Animator.GetBool((ColdOpen)))
                    sink.isRunning = false;
            }

            sink.Animator.SetBool(HotOpen, !sink.Animator.GetBool(HotOpen));
        }
        else
        {
            if (!Machine.isRunning)
            {
                sink.isHotWater = false;

                Machine.StartMachine();
            }
            else
            {
                if (!sink.Animator.GetBool(HotOpen) && sink.Animator.GetBool((ColdOpen)))
                    sink.isRunning = false;
                else if (!sink.Animator.GetBool((ColdOpen)))
                    sink.isHotWater = false;
                else if (sink.Animator.GetBool(HotOpen))
                    sink.isHotWater = true;
            }

            sink.Animator.SetBool(ColdOpen, !sink.Animator.GetBool(ColdOpen));
        }
    }
}