using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : Interactable
{
    [SerializeField] private bool playerDoor;
    [SerializeField] private GameObject door;
    [SerializeField] private Transform openTrans;
    private Vector3 _startposition;
    private Quaternion _startrotation;

    private bool _open;

    private bool _running;
    private bool _interrupt = true;

    private bool _occupied;

    public override void Start()
    {
        base.Start();
        var transform1 = transform;
        _startposition = transform1.position;
        _startrotation = transform1.rotation;
    }

    public override void OnInteract(PlayerInteraction interaction)
    {
        if (gameMode.gameModeData.isOpen) return;
        PlayerOpen();
    }

    private void PlayerOpen()
    {
        if (playerDoor && !_running)
        {
            Debug.Log("running: " + _running + " open: " + _open);
            if (!_running && _open)
            {
                _running = true;
            }
            else if (!_open)
            {
                _running = true;
            }
        }

        if (_running)
        {
            _interrupt = !_interrupt;
        }
    }

    private void Update()
    {
        if (!_running) return;
        switch (_open)
        {
            case false when !_interrupt:
            {
                door.transform.rotation =
                    Quaternion.Lerp(door.transform.rotation, openTrans.rotation, 2.5f * Time.deltaTime);
                if (door.transform.rotation == openTrans.rotation)
                {
                    _open = true;
                    _running = false;
                }

                break;
            }
            case false when _interrupt:
            {
                door.transform.rotation =
                    Quaternion.Lerp(door.transform.rotation, _startrotation, 2.5f * Time.deltaTime);
                if (door.transform.rotation == Quaternion.identity)
                {
                    _open = false;
                    _running = false;
                }

                break;
            }
            case true when !_interrupt:
            {
                door.transform.rotation =
                    Quaternion.Lerp(door.transform.rotation, _startrotation, 2.5f * Time.deltaTime);
                if (door.transform.rotation == Quaternion.identity)
                {
                    _open = false;
                    _running = false;
                }

                break;
            }
            default:
            {
                door.transform.rotation =
                    Quaternion.Lerp(door.transform.rotation, openTrans.rotation, 2.5f * Time.deltaTime);
                if (door.transform.rotation == openTrans.rotation)
                {
                    _open = true;
                    _running = false;
                }

                break;
            }
        }
    }

    public override void OnAltInteract(PlayerInteraction interaction)
    {
    }
}