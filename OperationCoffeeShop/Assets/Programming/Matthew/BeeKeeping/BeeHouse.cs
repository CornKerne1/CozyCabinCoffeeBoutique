using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BeeHouse : MonoBehaviour
{
    [SerializeField] private GameObject slabPrefab;
    [SerializeField] private Transform startTrans;
    [SerializeField] private int attractionChance;
    [SerializeField]public List<Slot> _slots = new List<Slot>();
    private Vector3 _lastPos;
    private GameMode _gameMode;
    private bool _occupied;
    private bool _colliding;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        DayNightCycle.HourChanged += AttractBees;
        CreateSlots();
    }

    private void AttractBees(object sender, EventArgs e)
    {
        if (_occupied) return;
    }

    private void CreateSlots()
    {
        for (var i = 0; i < 7; i++)
        {
            if (i == 0)
            {
                _lastPos = startTrans.position;
                _slots.Add(new Slot(_lastPos, i));
            }
            else
            {
                _lastPos = new Vector3(_lastPos.x, _lastPos.y + .1f, _lastPos.z);
                _slots.Add(new Slot(_lastPos, i));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(CO_Wait());
    }

    private IEnumerator CO_Wait()
    {
        yield return new WaitForSeconds(0.04f);
        _colliding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_colliding)
            return;
        _colliding = true;
        SlabInteractable slab;
        if (slab = other.transform.root.GetComponentInChildren<SlabInteractable>())//
        {
            foreach (Slot i in _slots)
            {
                if (!i.HasSlab())
                {
                    _gameMode.player.GetComponent<PlayerInteraction>().DropCurrentObj();
                    slab.GetComponent<Rigidbody>().isKinematic = true;
                    i.SetSlab(Instantiate(slabPrefab,i.GetPos(),Quaternion.identity));
                    GameObject slabObj;
                    (slabObj = slab.gameObject).transform.position = i.GetPos();
                    slabObj.transform.rotation = Quaternion.identity;
                    slab.GetComponent<Rigidbody>().isKinematic = true;
                    i.SetSlab(other.gameObject);
                    slab.slot = i;
                    slab.slabData = (SlabData)ScriptableObject.CreateInstance("SlabData");
                    i.slabData = slab.slabData;
                    slab.gameMode = _gameMode;
                    break;
                }
            }
        }
    }
    
    
}

[Serializable]public class Slot
{
    private Vector3 _pos;
    private int _slot;
    private GameObject _slab;
    public SlabData slabData;
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
