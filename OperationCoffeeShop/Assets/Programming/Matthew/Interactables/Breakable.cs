using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Interactable
{
    [SerializeField] private GameObject breakablePrefab;
    private PlayerInteraction _pI;
    private Rigidbody _rb;

    public override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        _pI = playerInteraction;
        _pI.Carry(gameObject);
    }

    IEnumerator CO_FreezeForClipping()
    {
        _rb.isKinematic = true;
        yield return new WaitForSeconds(.02f);
        var transform1 = transform;
        Instantiate(breakablePrefab, transform1.position, transform1.rotation);
        yield return new WaitForSeconds(.02f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var speed = _rb.velocity.magnitude*10f;
        if (speed >= gameMode.gameModeData.breakSpeed)
        {
            StartCoroutine(CO_FreezeForClipping());
        }
    }
}
