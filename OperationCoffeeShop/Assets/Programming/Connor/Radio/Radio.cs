using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Radio : Interactable
{
    [SerializeField] private Transform sT;
    [SerializeField] private Transform eT;
    [SerializeField] private GameObject radioChannel;
    public int currentChannel;
    private bool _isOn;

    [FormerlySerializedAs("RadioChannels")]
    public List<RadioChannel> radioChannels = new List<RadioChannel>();

    [SerializeField] private GameObject radioDial;

    [SerializeField] private int numberOfChannels = 9;

    public void PostSoundEvent(string s)
    {
        AkSoundEngine.PostEvent(s, gameObject);
    }

    public override void Start()
    {
        base.Start();
        for (var i = 0; i < numberOfChannels; i++)
        {
            Transform transform1;
            RadioChannel rC = Instantiate(radioChannel, (transform1 = transform).position, transform1.rotation)
                .GetComponent<RadioChannel>();
            rC.radio = this;
            rC.transform.SetParent(this.transform);
            radioChannels.Add(rC);
            rC.channel = radioChannels.Count - 1;
        }

        foreach (var rC in radioChannels)
        {
            rC.StartChannel();
        }

        currentChannel = Random.Range(0, radioChannels.Count);
        foreach (var rC in radioChannels)
            if (rC.channel == currentChannel)
                rC.PlayChannel();
            else
                rC.StopChannel();
        HandleDial();
    }

    private void HandleDial()
    {
        var position = sT.localPosition;
        var length = Mathf.Abs(position.x) - Mathf.Abs(eT.localPosition.x);
        var inc = -length * (1.0f / radioChannels.Count) * currentChannel;
        var localPosition = radioDial.transform.localPosition;
        radioDial.transform.localPosition = new Vector3(position.x - inc, localPosition.y,
            localPosition.z);
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.OnInteract(interaction);
        lookAtPlayer = true;
        LookAtPlayer();
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        currentChannel += 1;
        if (currentChannel > radioChannels.Count || currentChannel < 0)
            currentChannel = 0;
        foreach (var rC in radioChannels)
            if (rC.channel == currentChannel)
                rC.PlayChannel();
            else
                rC.StopChannel();
        HandleDial();
    }

    public override void ShowOnScreenText()
    {
        if (!_isBroken && playerInteraction)
            base.ShowOnScreenText();
    }

    public override void OnDrop()
    {
        base.OnDrop();
        lookAtPlayer = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        foreach (var rC in radioChannels)
        {
            rC.StopChannel();
            Destroy(rC.gameObject);
        }
    }
}