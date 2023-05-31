using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private GameMode gM;
    [SerializeField] private GameObject gate;
    [SerializeField] private Transform trans;
    [SerializeField] private Transform startPos;
    
    private bool activate;
    private bool running;
    private bool open;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        GameMode.ShopClosed += Closed;
        gate.SetActive(false);
    }

    private void Closed(object sender, EventArgs e)
    {
        open = false;
        activate = true;
        gate.SetActive(false);
    }

    public void OpenGate()
    {
        open = true;
        activate = true;
        gate.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            if (open)
            {
                running = true;
                gate.transform.position = Vector3.Lerp(gate.transform.position, trans.position, 1*Time.deltaTime);

            }
            else
            {
                running = true;
                gate.transform.position = Vector3.Lerp(gate.transform.position, startPos.position, 1*Time.deltaTime);
            }
        }
    }
}
