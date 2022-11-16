using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
[Serializable]
public class DeliveryManager
{
    private readonly DeliveryManager _deliveryManager;
    private readonly GameMode _gameMode;
    private readonly GameModeData _gameModeData;
    private Queue<DeliveryData> _deliveries;
    public enum ObjType { Coffee, Espresso, Milk, Sugar,Camera,PictureFrame}

    public DeliveryManager(DeliveryManager deliveryManager, GameMode gameMode, GameModeData gameModeData)
    {
        _deliveries = new Queue<DeliveryData>();
        _deliveryManager = deliveryManager;
        _gameMode = gameMode;
        _gameModeData = gameModeData;
        gameModeData.deliveryQueued = false;
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
            condition = d != null;
            break;
        }

        return condition;
    }
    private void CreateNewDeliveryData()
    {
        _deliveries.Enqueue( new DeliveryData());
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

    public Queue<DeliveryData> GetQueue()
    {
        return _deliveries;
    }
    public void SetQueue(Queue<DeliveryData> deliveries)
    {
        deliveries = _deliveries;
    }
}
[Serializable]
public class DeliveryData
{
    private List<DeliveryPackage> _deliveryPackages;
    private int _maxPackages = 16;
    public DeliveryData()
    {
        _deliveryPackages = new List<DeliveryPackage>();
    }
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

}
[Serializable]
public class DeliveryPackage
{
    public DeliveryPackage(DeliveryManager.ObjType objType, int quantity)
    {
        this.objType = objType;
        this.quantity = quantity;
    }
    public DeliveryManager.ObjType objType;
    public int quantity;
}
