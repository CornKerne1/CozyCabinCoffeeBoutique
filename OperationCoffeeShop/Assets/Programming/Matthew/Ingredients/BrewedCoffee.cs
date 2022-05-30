using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrewedCoffee : MonoBehaviour
{
    public float maxScale;
    public GameObject matOwner;
    private Material mat;
    public float speed;
    private bool changeColor;
    private bool animate;
    float timer;
    public float maxTimer;

    public IngredientNode iN;

    private void Awake()
    {
    }

    private void Start()
    {
        timer = maxTimer;
        changeColor = true;
        var myMat = GetComponent<MeshRenderer>().materials;
        var newMat = GetComponent<MeshRenderer>().materials[0];
        myMat[0] = new Material(newMat);
        GetComponent<MeshRenderer>().materials = myMat;
        mat = matOwner.GetComponent<Renderer>().material;
        mat.SetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e", 0.0f); //
    }

    private void Update()
    {
        mat.SetFloat("Vector1_509ce15df0f245ffba027f51d8eaef81",
            mat.GetFloat("Vector1_509ce15df0f245ffba027f51d8eaef81") - .0035f);
        if (mat.GetFloat("Vector1_509ce15df0f245ffba027f51d8eaef81") <= 0)
        {
            Destroy(gameObject);

        }

        ChangeColor();
        ScaleMesh();
        timer = -Time.deltaTime;

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
                mat.SetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e",
                    mat.GetFloat("Vector1_f635bf8842f4453fa95dcb17f6f4ad4e") + (speed * 5f));
            }
        }
    }

    private void ScaleMesh()
    {
        if (timer <= 0)
        {
            if (transform.localScale.x > 0 || transform.localScale.z < maxScale)
            {
                transform.localScale -= new Vector3(.0001f, 0, 0);
                transform.localScale += new Vector3(0, .00028f, .00028f);
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

    private void TryAddOrDelete(GameObject obj)
    {
        try
        {
            obj.GetComponent<IngredientContainer>().AddToContainer(iN); //WRITE CODE THAT CHECKS IF THIS INGREDIENT IS ALREADY ON LIST. IF SO ONLY USE THE AMOUNT AND DONT ADD THE ARRAY ELEMENT;
            Destroy(gameObject);
        }
        catch
        {
            Destroy(gameObject);
        }
    }
}

