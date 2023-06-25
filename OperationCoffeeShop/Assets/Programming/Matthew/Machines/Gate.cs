using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private GameMode gM;
    [SerializeField] private GameObject gate;
    [SerializeField] private Transform trans;
    private Transform startPos;
    // Start is called before the first frame update
    void Start()
    {
        var t = transform;
        startPos = t;
        gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        GameMode.ShopClosed += Closed;
        gate.SetActive(false);
    }

    private async void Closed(object sender, EventArgs e)
    {
        await MoveGate(false);
    }

    public async void OpenGate()
    {
        gate.SetActive(true);
        await MoveGate(true);
    }

    private async Task MoveGate(bool open)
    {
        float currentTime = 0;
        while (currentTime<5f)
        {
            currentTime += Time.deltaTime;
            gate.transform.position = Vector3.Lerp(gate.transform.position, open ? trans.position : startPos.position, 1*Time.deltaTime);
            await Task.Yield();
        }
        if(!open)gate.SetActive(false);
    }

    private void OnDestroy()
    {
        GameMode.ShopClosed -= Closed;
    }
}
