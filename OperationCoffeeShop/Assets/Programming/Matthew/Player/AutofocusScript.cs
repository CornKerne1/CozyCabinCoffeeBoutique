using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AutofocusScript : MonoBehaviour
{
    
    DepthOfField dofComponent;
    [SerializeField] public Volume volume;


    public void UpdateFocus(float distance)
    {
        dofComponent.focusDistance = new MinFloatParameter(distance, 1.5f, true);
        volume.profile.isDirty = true;
    }
    void Start()
    {
        DepthOfField tmp;
        if (volume.profile.TryGet<DepthOfField>(out tmp))//
        {
            dofComponent = tmp;
        }
    }
}
