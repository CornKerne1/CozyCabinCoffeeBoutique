using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadingFire : MonoBehaviour
{
    private IEnumerator _coRoutine;
    private float _loopInterval=1f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject firePrefab;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_coRoutine == null)
            StartCoroutine(CO_SpreadFire());
    }

    IEnumerator CO_SpreadFire()
    {
        _coRoutine = CO_SpreadFire();
        SpreadFire();
        yield return new WaitForSeconds(_loopInterval);
        _coRoutine = null;
    }
    private void SpreadFire()
    {
        var roll = Random.Range(0, 3);
        if (roll == 0)
        {
            roll = Random.Range(0, 3);
            switch (roll)
            {
                case 0:
                    SpawnFire(-.5f,0);
                    break;
                case 1:
                    SpawnFire(.5f,0);
                    break;
                case 2:
                    SpawnFire(0,-.5f);
                    break;
                case 3:
                    SpawnFire(0,.5f);
                    break;
            }
        }
    }

    private void SpawnFire(float x, float z)
    {
        var start = new Vector3(transform.position.x+x, transform.position.y + .4f, transform.position.z+z);
        RaycastHit hit;
        if (Physics.Raycast(start, Vector3.down, out hit, 1f, _layerMask))
        {
            Instantiate(firePrefab, hit.transform);
            firePrefab.transform.position = hit.transform.position;
            firePrefab.transform.rotation = hit.transform.rotation;
        }
        else SpreadFire();
    }
}
