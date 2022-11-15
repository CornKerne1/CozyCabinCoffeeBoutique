using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DeliveryManager
{
    private readonly DeliveryManager _deliveryManager;
    private readonly GameMode _gameMode;
    private readonly GameModeData _gameModeData;
    private Queue<DeliveryData> _deliveries;

    public DeliveryManager(DeliveryManager deliveryManager, GameMode gameMode, GameModeData gameModeData)
    {
        _deliveryManager = deliveryManager;
        _gameMode = gameMode;
        _gameModeData = gameModeData;
        _deliveries = new Queue<DeliveryData>();
    }

    public DeliveryData GetDelivery()
    {
        if (_deliveries.Count <= 1)
            _gameModeData.deliveryQueued = false;
        return _deliveries.Dequeue();
    }
    private bool HasDeliveryData()
    {
        var condition = false;
        foreach (var d in _deliveries)
        {
            condition=d;
            break;
        }

        return condition;
    }

    private void CreateNewDeliveryData()
    {
        _deliveries.Enqueue( (DeliveryData)ScriptableObject.CreateInstance("DeliveryData"));
        _gameModeData.deliveryQueued = true;
    }

    private void ModifyDelivery(DeliveryPackage deliveryPackage)
    {
        var success = false;
        foreach (var d in _deliveries.Where(d => !d.IsFull()))
        {
            d.AddToList(deliveryPackage);
            success = true;
            break;
        }
        if (success) return;
        CreateNewDeliveryData();
        AddToDelivery(deliveryPackage);
    }

    public void AddToDelivery(DeliveryPackage deliveryPackage)
    {
        if(HasDeliveryData())
            ModifyDelivery(deliveryPackage);
        else
        {
            CreateNewDeliveryData();
            ModifyDelivery(deliveryPackage);
        }
    }
}
public class DeliveryData : ScriptableObject
{
    private List<DeliveryPackage> _deliveryPackages;
    private int _maxPackages = 16;
    public void AddToList(DeliveryPackage i)
    {
        _deliveryPackages.Add(i);
    }

    public List<DeliveryPackage> GetDeliveryPackages()
    {
        return _deliveryPackages;
    }
    public bool IsFull()
    {
        return _deliveryPackages.Count >= _maxPackages;
    }
    private void OnEnable()
    {
        _deliveryPackages = new List<DeliveryPackage>();
    }
}
public class DeliveryPackage
{
    public DeliveryPackage(string objType, int quantity)
    {
        this.objType = objType;
        this.quantity = quantity;
    }
    public string objType;
    public int quantity;
}
