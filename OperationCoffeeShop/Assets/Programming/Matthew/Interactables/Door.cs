using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private Transform _startTrans;

    [SerializeField] private Transform openTrans;

    [SerializeField] private bool playerDoor;

    private bool _open;

    private bool _running;

    private bool _occupied;

    private void Start()
    {
        _startTrans = transform;
    }

    public void PlayerOpen()
    {
        if (playerDoor)
        {
            if (_running && _open)
            {
                _open = false;
            }
            else if (!_open)
            {
                _running = true;
            }
        }
    }

    private void Update()
    {
        if (_running)
        {
            if (!_open)
            {
                door.transform.rotation =
                    Quaternion.Lerp(door.transform.rotation, openTrans.rotation, 2.5f * Time.deltaTime);
                if (door.transform.rotation == openTrans.rotation)
                {
                    _open = true;
                    _running = false;
                    StartCoroutine(TryClose());
                }
            }
            else
            {
                door.transform.rotation =
                    Quaternion.Lerp(door.transform.rotation, _startTrans.rotation, 2.5f * Time.deltaTime);
                if (door.transform.rotation == _startTrans.rotation)
                {
                    _open = false;
                    _running = false;
                }
            }
        }
    }

    private IEnumerator TryClose()
    {
        yield return new WaitForSeconds(1f);
        if (!_occupied)
        {
            _running = true;
        }
        else
        {
            StartCoroutine(TryClose());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerDoor)
        {
            if (_running && _open)
            {
                _open = false;
            }
            else if (!_open)
            {
                _running = true;
            }
        }
        else if (other.CompareTag("Customer"))
        {
            if (_running && _open)
            {
                _open = false;
            }
            else if (!_open)
            {
                AkSoundEngine.PostEvent("PLAY_SFX_BELLCHIME", gameObject);
                _running = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            _occupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            _occupied = false;
        }
    }
}