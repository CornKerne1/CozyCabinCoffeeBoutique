using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TreeRandomizer : MonoBehaviour
{
    private Material mat;
    [SerializeField] private int matSlot = 0;
    private int count;
    
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
        var r = Random.Range(0.3f, 1.0f);
        var g = Random.Range(0.5f, 1.0f);
        var b = Random.Range(0.1f, 0.45f);
        var a = 1;
        var rndr = GetComponent<MeshRenderer>();
        var myMat = rndr.materials;
        var newMat = rndr.materials[matSlot];
        newMat.SetColor("_BaseColor", new Color(r,g,b,a));
        foreach (Material m in rndr.materials)
        {
            if (m == rndr.materials[matSlot])
            {
                myMat[matSlot] = new Material(newMat);
            }
            else
            {
                var newMat1 = rndr.materials[count];
                myMat[count] = new Material(newMat1);
            }
            count = count + 1;
        }
        rndr.materials = myMat;
    }
}