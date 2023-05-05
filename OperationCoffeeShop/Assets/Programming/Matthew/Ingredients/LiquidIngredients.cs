using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidIngredients : MonoBehaviour
{
    public IngredientNode ingredientNode;
    public Color colorInDrink;
    [SerializeField]private GameObject matOwner;
    [SerializeField]private float maxAnimationScale=0.2f;
    [SerializeField]private float animationSpeed=.001f;
    [SerializeField]private float maxAnimationTimer=1.5f;
    private Material _mat;
    private bool _changeColor;
    private bool _animate = true;
    private float _timer;

    private void Start()
    {
        _timer = maxAnimationTimer;
        _changeColor = true;
        var myMat = GetComponent<MeshRenderer>().materials;
        var newMat = GetComponent<MeshRenderer>().materials[0];
        myMat[0] = new Material(newMat);
        GetComponent<MeshRenderer>().materials = myMat;
        _mat = matOwner.GetComponent<Renderer>().material;
        _mat.SetFloat("Lifetime", 0.0f); //
    }

    private async void Update()
    {
        if (!_animate) return;
        _mat.SetFloat("Alpha",
            _mat.GetFloat("Alpha") - .0035f);
        if (_mat.GetFloat("Alpha") <= 0)
        {
            Destroy(gameObject);
        }

        ChangeColor();
        ScaleMesh();
        _timer = -Time.deltaTime;

    }

    private async void ChangeColor()
    {
        if (_changeColor)
        {
            if (_mat.GetFloat("Lifetime") >= 1)
            {
                _changeColor = false;
            }
            else
            {
                _mat.SetFloat("Lifetime",
                    _mat.GetFloat("Lifetime") + (animationSpeed * 5f));
            }
        }
    }

    private async void ScaleMesh()
    {
        if (_timer <= 0)
        {
            if (transform.localScale.x > 0 || transform.localScale.z < maxAnimationScale)
            {
                transform.localScale -= new Vector3(.0001f, 0, 0);
                transform.localScale += new Vector3(0, .00028f, .00028f);
            }
            else
            {
                _animate = false;
                Destroy(gameObject);
            }
        }
    }
    protected virtual async void OnTriggerEnter(Collider other)
    {
        TryAddOrDelete(other.gameObject);
    }

    protected virtual async void TryAddOrDelete(GameObject obj)
    {
        try
        {
            obj.GetComponent<IngredientContainer>()
                .AddToContainer(
                    ingredientNode,colorInDrink); //WRITE CODE THAT CHECKS IF THIS INGREDIENT IS ALREADY ON LIST. IF SO ONLY USE THE AMOUNT AND DONT ADD THE ARRAY ELEMENT;
            Destroy(gameObject);
        }
        catch
        {
            Destroy(gameObject);
        }
    }
}
