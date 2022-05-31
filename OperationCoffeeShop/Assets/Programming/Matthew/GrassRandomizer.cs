using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GrassRandomizer : MonoBehaviour
{
    private Material mat;
    [SerializeField] private int matSlot = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        var r = Random.Range(0.01f, .7f);
        var g = Random.Range(0.4f, .75f);
        var b = Random.Range(0.01f, 0.25f);
        var a = 1;
        var myMat = GetComponent<MeshRenderer>().materials;
        var newMat = GetComponent<MeshRenderer>().materials[matSlot];
        newMat.SetColor("_Color", new Color(r,g,b,a));
        myMat[0] = new Material(newMat);
        GetComponent<MeshRenderer>().materials = myMat;
    }
}