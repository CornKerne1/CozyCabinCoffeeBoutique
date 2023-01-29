using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mop : Interactable
{
    // Start is called before the first frame update
    private bool _cleaning = false;
    [SerializeField] private float sweepSpeed = 2f;
    [SerializeField] private float distance = .2f;
    public Transform _playerTransform;

    private Vector3 startLocalPosition;

    private PlayerInteraction _interaction;
    
    public override void Start()
    {
        base.Start();
        _playerTransform = gameMode.player;
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        base.playerInteraction = interaction;
        interaction.Carry(gameObject);
    }

    public override void OnDrop()
    {
        base.OnDrop();
        _cleaning = false;
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
        _interaction = interaction;
        if (!_cleaning)
        {
            _cleaning = true;
            startLocalPosition = transform.localPosition - _interaction.carriedObjPosition;
            StartCoroutine(ToggleMop());
        }
        else
        {
            _cleaning = false;
            transform.rotation = Quaternion.identity;
        }
    }
    

    private IEnumerator ToggleMop()
    {
        while (_cleaning)
        {
            float newX = Mathf.Sin(Time.time * sweepSpeed) * distance + startLocalPosition.x;
            transform.position = _interaction.carriedObjPosition + new Vector3(newX, startLocalPosition.y, startLocalPosition.z);

            yield return null;
        }
    }

}