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
    private bool animate;
    
    public IngredientNode iN;

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
        mat.SetFloat("Vector1_509ce15df0f245ffba027f51d8eaef81", mat.GetFloat("Vector1_509ce15df0f245ffba027f51d8eaef81") -.002f);
        ChangeColor();
        ScaleMesh();

    }

    private void ChangeColor()
    {
        if (changeColor)
        {
            if (mat.GetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e") >= 1)
            {
                changeColor = false;
            }
            else
            {
                mat.SetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e", mat.GetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e") + (speed* 5f));
            }
        }
    }

    private void ScaleMesh()
    {
        if (animate)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(.0001f, 0, 0);
                transform.localScale += new Vector3(0, .0005f, .0005f);
            }
            else
            {
                animate = false;
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        TryAddOrDelete(other.gameObject);
        
    }

    void OnCollisionEnter(Collision collision)
    {
        animate = true;
    }

    private void TryAddOrDelete(GameObject obj)
    {
        try
        {
            obj.GetComponent<IngredientContainer>().AddToContainer(iN);//WRITE CODE THAT CHECKS IF THIS INGREDIENT IS ALREADY ON LIST. IF SO ONLY USE THE AMOUNT AND DONT ADD THE ARRAY ELEMENT;
            Destroy(gameObject);
        }
        catch{Destroy(gameObject);}
    }
}
