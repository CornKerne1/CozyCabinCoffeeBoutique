using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TreeRandomizer : MonoBehaviour
{
    [SerializeField] private Material matL;
    [SerializeField] private Material matM;
    [SerializeField] private Material matD;

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
        matL = gM.gMD.matL;
        matM = gM.gMD.matM;
        matD = gM.gMD.matD;
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