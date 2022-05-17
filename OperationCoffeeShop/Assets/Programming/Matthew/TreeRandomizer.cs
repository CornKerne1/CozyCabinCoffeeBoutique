using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TreeRandomizer : MonoBehaviour
{

    private GameMode gM;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        int random = Random.Range(0,2);
        var matL = gM.gMD.treeMats[0];
        var matM = gM.gMD.treeMats[1];
        var matD = gM.gMD.treeMats[2];
        var myMat = GetComponent<MeshRenderer>().materials;
        switch(random)
        {
            case 0:
                myMat[0] = matL;
                GetComponent<MeshRenderer>().materials = myMat;
                break;
            case 1:
                myMat[0] = matM;
                GetComponent<MeshRenderer>().materials = myMat;
                break;
            case 2:
                myMat[0] = matD;
                GetComponent<MeshRenderer>().materials = myMat;
                break;
        }
    }
}