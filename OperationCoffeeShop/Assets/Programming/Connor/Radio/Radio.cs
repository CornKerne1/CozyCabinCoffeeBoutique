using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : Interactable
{
    [SerializeField] private Transform sT;
    [SerializeField] private Transform eT;
    [SerializeField] private GameObject radioChannel;
    public int currentChannel;
    bool isOn;
    public List<RadioChannel> RadioChannels = new List<RadioChannel>();
    [SerializeField] private GameObject radioDial;
    PlayerInteraction pI;//
    public void PostSoundEvent(string s) { AkSoundEngine.PostEvent(s, this.gameObject); }
    public override void Start()
    {
        base.Start();
        for (int i = 0; i < 11; i++)
        {
            var rC = Instantiate(radioChannel, transform.position, transform.rotation).GetComponent<RadioChannel>();
            rC.channel = i;
            rC.radio = this;
            RadioChannels.Insert(i,rC);
            rC.transform.SetParent(this.transform);
        }
        foreach (RadioChannel rC in RadioChannels)
        {
            rC.StartChannel();
        }
        currentChannel = Random.Range(0, RadioChannels.Count);
        RadioChannels[currentChannel].PlayChannel();
        HandleDial();
    }

    public void HandleDial()
    {
        var length = Mathf.Abs(sT.position.x) - Mathf.Abs(eT.position.x);
        var inc = -length * (1.0f / RadioChannels.Count) * currentChannel;
        radioDial.transform.position = new Vector3(sT.position.x - inc, radioDial.transform.position.y,
            radioDial.transform.position.z);
    }
    public override void OnFocus()
    {
        
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
        {
            currentChannel = 0;
        }

        foreach (RadioChannel rC in RadioChannels)
        {
            if (rC.channel == currentChannel)
            {
                rC.PlayChannel();
            }
            else
            {
                rC.StopChannel();
            }
        }

        HandleDial();
    }
    
    public override void OnLoseFocus()
    {
    }
}
