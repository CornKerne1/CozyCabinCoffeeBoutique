using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private Transform _startTrans;
    [SerializeField] private Transform openTrans;
    private bool _open;
    private bool _running;
    private bool _occupied;
    private void Start()
    {
        _startTrans = transform;
    }

    public void PlayerOpen()
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

    private void Update()
    {
        if (!_running) return;
        if (!_open)
        {
            door.transform.rotation =
                Quaternion.Lerp(door.transform.rotation, openTrans.rotation, 2.5f * Time.deltaTime);
            Debug.Log("DoorMoving");

            if (door.transform.rotation != openTrans.rotation) return;

            _open = true;
            _running = false;
            StartCoroutine(TryClose());
        }
        else
        {
            door.transform.rotation =
                Quaternion.Lerp(door.transform.rotation, _startTrans.rotation, 2.5f * Time.deltaTime);

            if (door.transform.rotation != _startTrans.rotation) return;

            _open = false;
            _running = false;
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
        if (other.CompareTag("Customer"))
        {
            Debug.Log("trigger open door for customer");

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