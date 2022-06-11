using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Radio : Interactable
{
    [SerializeField] private Transform sT;
    [SerializeField] private Transform eT;
    [SerializeField] private GameObject radioChannel;
    public int currentChannel;
    bool isOn;
    public List<RadioChannel> RadioChannels = new List<RadioChannel>();//
    [SerializeField] private GameObject radioDial;
    PlayerInteraction pI;//
    public void PostSoundEvent(string s) { AkSoundEngine.PostEvent(s, this.gameObject); }
    public override void Start()
    { 
        for (int i = 0; i < 10; i++)
        {
                RadioChannel rC = Instantiate(radioChannel, transform.position, transform.rotation).GetComponent<RadioChannel>();
                    rC.radio = this;
                    rC.transform.SetParent(this.transform);
                    RadioChannels.Add(rC);
                    rC.channel = RadioChannels.Count;
        }
        foreach (var rC in RadioChannels)
        {
            rC.StartChannel();
        }
        currentChannel = Random.Range(0, RadioChannels.Count);
        RadioChannels[currentChannel].PlayChannel();
        HandleDial();
    }
    public void HandleDial()
    {
        var position = sT.localPosition;
        var length = Mathf.Abs(position.x) - Mathf.Abs(eT.localPosition.x);
        var inc = -length * (1.0f / RadioChannels.Count) * currentChannel;
        var localPosition = radioDial.transform.localPosition;
        radioDial.transform.localPosition = new Vector3(position.x - inc, localPosition.y,
            localPosition.z);
    }
    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
        pI.Carry(gameObject);
    }
    public override void OnAltInteract(PlayerInteraction pI)
    {
        currentChannel = currentChannel + 1;
        if (currentChannel > RadioChannels.Count ||currentChannel < 0)
            currentChannel = 0;
        foreach (var rC in RadioChannels)
        {
            if (rC.channel == currentChannel)
                rC.PlayChannel();
            else
                rC.StopChannel();
        }
        HandleDial();
    }
}
