using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxModule : MonoBehaviour
{

    [SerializeField] private Gradient _skyColor;
    [SerializeField] private Gradient _horizonColor;
    void Start()
    {
        RenderSettings.skybox.SetColor("_SkyTint",_skyColor.Evaluate(.5f));
        RenderSettings.skybox.SetColor("_GroundColor",_horizonColor.Evaluate(.5f));
    }
}
