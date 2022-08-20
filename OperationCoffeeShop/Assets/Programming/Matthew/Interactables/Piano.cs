using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Piano : Interactable
{
    [FormerlySerializedAs("radioChannel")] [SerializeField]
    private GameObject pianoChannel;

    public int currentChannel;
    private bool _isOn;

    [FormerlySerializedAs("radioChannels")]
    public List<PianoChannel> pianoChannels = new List<PianoChannel>();

    private bool _on;

    private Animator _animator;
    private static readonly int On = Animator.StringToHash("On");


    [FormerlySerializedAs("_numberOfChannels")] [SerializeField]
    private int numberOfChannels = 2;

    public override void Start()
    {
        base.Start();
        _animator = gameObject.GetComponent<Animator>();
        for (var i = 0; i < numberOfChannels; i++)
        {
            Transform transform1;
            var pC = Instantiate(pianoChannel, (transform1 = transform).position, transform1.rotation)
                .GetComponent<PianoChannel>();
            pC.piano = this;
            pC.transform.SetParent(this.transform);
            pianoChannels.Add(pC);
            pC.channel = pianoChannels.Count - 1;
        }

        foreach (var pC in pianoChannels)
        {
            pC.StartChannel();
          
        }

        currentChannel = Random.Range(0, pianoChannels.Count);
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        if (_on)
        {
            _on = false;
            _animator.SetBool(On, false);
            if (currentChannel > pianoChannels.Count || currentChannel < 0)
                currentChannel = 0;
            foreach (var p in pianoChannels.Where(p => p.channel == currentChannel))
                p.StopChannel();
        }
        else
        {
            _on = true;
            _animator.SetBool(On, true);
            currentChannel += 1;
            if (currentChannel >= pianoChannels.Count || currentChannel < 0)
                currentChannel = 0;
            foreach (var p in pianoChannels)
                if (p.channel == currentChannel)
                    p.PlayChannel();
                else
                    p.StopChannel();
        }
    }
}