using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrewedCoffee : MonoBehaviour
{
    public GameObject matOwner;
    private Material mat;
    public float speed;
    private bool changeColor;

    private void Awake()
    {
        mat = matOwner.GetComponent<Renderer>().material;
    }

    private void Start()
    {
        changeColor = true;
        mat.SetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e", 0.0f);
    }
    private void Update()
    {
        Debug.Log(mat.GetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e"));
        if (changeColor)
        {
            if (mat.GetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e") >= 1)
            {
                changeColor = false;
            }
            else
            {
                mat.SetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e", mat.GetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e") + (speed* 20f));
            }
        }
    }
    
}
