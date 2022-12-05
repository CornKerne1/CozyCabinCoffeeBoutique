using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SaveGameData
{
   public List<RespawbableData> respawnables;
   public float playerMoney;
   public Vector3 playerPosition;
   public int savedHour,savedDay,savedMonth,savedYear;
   public List<DeliveryPackage> deliveryPackages;
   public bool completedTutorial;
   
   public SaveGameData()
   {
      
   }
}
[Serializable]
public class RespawbableData
{
   public DeliveryManager.ObjType objType;
   public Vector3 position;
   public Quaternion rotation;
   public int wildCard;
   public RespawbableData(DeliveryManager.ObjType objType,Vector3 position,Quaternion rotation,int wildCard)
   {
      this.objType = objType;
      this.position = position;
      this.rotation = rotation;
      this.wildCard = wildCard;
   }
}

[Serializable]
public class SaveOptionsData
{
   public float masterVol;
   public float musicVol;
   public float sfxVol;
   public bool performanceMode;
}