using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private GameMode gM;
    [SerializeField] private GameObject gate;
    [SerializeField] private Transform trans;
    [SerializeField] private Transform startTrans;
    
    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        startTrans = gate.transform;
    }

    public void OpenCloseGate()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (gM.gMD.isOpen)
            {//Open
                gate.transform.position = Vector3.Lerp(gate.transform.position, trans.position, 1);
                if (gate.transform.position == trans.position)
                {
                    active = false;
                }
            }
            else
            {//Close
                gate.transform.position = Vector3.Lerp(gate.transform.position, startTrans.position, 1);
                if (gate.transform.position == startTrans.position)
                {
                    active = false;
                }
            }
        }
    }
}
