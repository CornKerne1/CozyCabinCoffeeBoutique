using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TreeRandomizer : MonoBehaviour
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
        var r = Random.Range(.35f, .65f);
        var g = Random.Range(.35f, .65f);
        var b = Random.Range(.35f, .65f);
        var a = Random.Range(0f, 1f);
        var myMat = GetComponent<MeshRenderer>().materials;
        var newMat = GetComponent<MeshRenderer>().materials[matSlot];
        newMat.SetColor("_BaseColor", new Color(r,g,b,a));
        myMat[0] = new Material(newMat);
        GetComponent<MeshRenderer>().materials = myMat;
    }
}