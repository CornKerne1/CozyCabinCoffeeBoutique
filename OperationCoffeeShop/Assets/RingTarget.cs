using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RingTarget : MonoBehaviour
{
    public static event EventHandler TargetBroken;

    private void OnTriggerEnter(Collider other)
    {

        TargetBroken?.Invoke(this.gameObject, EventArgs.Empty);
    }
}

