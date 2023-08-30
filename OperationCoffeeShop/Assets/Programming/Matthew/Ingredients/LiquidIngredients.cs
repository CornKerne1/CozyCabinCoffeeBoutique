using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LiquidIngredients : MonoBehaviour
{
    public IngredientNode ingredientNode;
    public Color colorInDrink;
    [SerializeField]private GameObject matOwner;
    [SerializeField]private float maxAnimationScale=0.2f;
    [SerializeField]private float animationSpeed=.001f;
    [SerializeField]private float maxAnimationTimer=1.5f;
    [SerializeField]private bool changeColor =true;
    private Material _mat;
    private bool _animate = true;
    private float _timer;
    private static readonly int Lifetime = Shader.PropertyToID("Lifetime");
    private static readonly int Alpha = Shader.PropertyToID("Alpha");

    private void Start()
    {
        _timer = maxAnimationTimer;
        var myMat = GetComponent<MeshRenderer>().materials;
        var newMat = GetComponent<MeshRenderer>().materials[0];
        myMat[0] = new Material(newMat);
        GetComponent<MeshRenderer>().materials = myMat;
        _mat = matOwner.GetComponent<Renderer>().material;
        _mat.SetFloat(Lifetime, 0.0f); //
    }

    private async void Update()
    {
        if (!_animate) return;
        _mat.SetFloat(Alpha,
            _mat.GetFloat(Alpha) - .0035f);
        if (_mat.GetFloat(Alpha) <= 0)
        {
            Destroy(gameObject);
        }

        ChangeColor();
        ScaleMesh();
        _timer = -Time.deltaTime;

    }

    private async void ChangeColor()
    {
        if (changeColor)
        {
            if (_mat.GetFloat(Lifetime) >= 1)
            {
                changeColor = false;
            }
            else
            {
                _mat.SetFloat(Lifetime,
                    _mat.GetFloat(Lifetime) + (animationSpeed * 5f));
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
        var iC = obj.GetComponent<IngredientContainer>();
        if (iC)
        {
            await iC.AddToContainer(ingredientNode, colorInDrink);
            Destroy(gameObject);
        }
    }
}
