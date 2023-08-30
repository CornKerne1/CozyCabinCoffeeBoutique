using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class TreeRandomizer : MonoBehaviour
{
    private Material mat;
    [SerializeField] private int matSlot = 0;
    private int count;
    
    // Start is called before the first frame update
    async void Start()
    {
        await Initialize();
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async Task Initialize()
    {
        var r = Random.Range(0.3f, 1.0f);
        var g = Random.Range(0.5f, 1.0f);
        var b = Random.Range(0.1f, 0.45f);
        var a = 1;
        if (g > .7f && r > .7f)
        {
            g = g - Random.Range(.15f, .35f);
            if (r > .8f)
            {
                r = r - Random.Range(.1f, .2f);
            }
        }
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