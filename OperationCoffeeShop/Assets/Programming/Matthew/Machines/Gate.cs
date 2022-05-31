using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private GameMode gM;
    [SerializeField] private GameObject gate;
    [SerializeField] private Transform trans;
    [SerializeField] private Transform startTrans;
    
    private bool activate;
    private bool running;
    private bool open;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        var sT = gate.transform;
        startTrans = sT;
        GameMode.ShopClosed += Closed;
    }

    private void Closed(object sender, EventArgs e)
    {
        OpenGate();
    }

    void Closed()
    {
        if (!running)
        {
            open = false;
            activate = true;
        }
    }
    public void OpenGate()
    {
        if (!running)
        {
            open = true;
            activate = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            if (open)
            {
                running = true;
                gate.transform.position = Vector3.Lerp(gate.transform.position, trans.position, 1);
                if (gate.transform.position == trans.position)
                {
                    activate = false;
                    running = false;
                }

            }
            else
            {
                running = true;
                gate.transform.position = Vector3.Lerp(gate.transform.position, startTrans.position, 1);
                if (gate.transform.position == startTrans.position)
                {
                    activate = false;
                    running = false;
                }
            }
        }
    }
}
