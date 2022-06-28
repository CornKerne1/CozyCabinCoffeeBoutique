using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BeeHouse : MonoBehaviour
{
    [SerializeField] private GameObject slabPrefab;
    [SerializeField] private Transform startTrans;
    private List<Slot> _slots = new List<Slot>();
    private Vector3 _lastPos;
    private GameMode _gameMode;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        CreateSlots();
    }

    private void CreateSlots()
    {
        for (int i = 0; i >= 6; i++)
        {
            if (i == 0)
            {
                _lastPos = startTrans.position;
                _slots[i] = new Slot(_lastPos, i);
            }
            else
            {
                _lastPos = new Vector3(_lastPos.x, _lastPos.y + 2, _lastPos.z);
                _slots[i] = new Slot(_lastPos, i);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.GetComponentInChildren<SlabInteractable>())
        {
            foreach (Slot i in _slots)
            {
                if (!i.HasSlab())
                {
                    i.SetSlab(Instantiate(slabPrefab,i.GetPos(),Quaternion.identity));
                    var slab = i.GetSlab().transform.root.GetComponentInChildren<Slab>();
                    slab.slot = i;
                    slab.slabData = (SlabData)ScriptableObject.CreateInstance("SlabData");
                    slab.gameMode = _gameMode;
                    break;
                }
            }
        }
    }
}

public class Slot
{
    private Vector3 _pos;
    private int _slot;
    private GameObject _slab;
    //SLABDATA
    public Slot(Vector3 pos, int slot)
   {
       _slot = slot;
       _pos = pos;
   }

    public bool HasSlab() { return _slab; }
    public Vector3 GetPos() { return _pos; }
    public void SetSlab(GameObject slab)
    {
        _slab = slab;
    }
    public GameObject GetSlab()
    {
        return _slab;
    }
}
