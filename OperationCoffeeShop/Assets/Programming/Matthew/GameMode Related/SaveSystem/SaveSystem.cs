using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private SaveSystem _saveSystem;
    private GameMode _gameMode;
    private GameModeData _gameModeData;
    public SaveOptionsData SaveOptionsData;
    
    public SaveGameData SaveGameData;
    
    
    public static event EventHandler SaveGameEvent;
    public SaveSystem(SaveSystem saveSystem, GameMode gameMode, GameModeData gameModeData)
    {
        _saveSystem = saveSystem;
        _gameMode = gameMode;
        _gameModeData = gameModeData;
    }
    public void Initialize()
    {
        if (SaveOptionsData != null) return;
        SaveOptionsData = new SaveOptionsData();
        var gameNumber = 0;
        if (File.Exists(Application.persistentDataPath + $"SaveOptions{gameNumber}.json"))
        {
            using (StreamReader streamReader =
                   new StreamReader(Application.persistentDataPath + $"SaveOptions{gameNumber}.json"))
            {
                var json = streamReader.ReadToEnd();
                SaveOptionsData = JsonUtility.FromJson<SaveOptionsData>(json);
            }

            AkSoundEngine.SetRTPCValue("MasterVolume", SaveOptionsData.masterVol);
            AkSoundEngine.SetRTPCValue("MusicVolume", SaveOptionsData.musicVol);
            AkSoundEngine.SetRTPCValue("SFXVolume", SaveOptionsData.masterVol);
        }
        else
        {
            AkSoundEngine.SetRTPCValue("MasterVolume", .4f);
            AkSoundEngine.SetRTPCValue("MusicVolume", .4f);
            AkSoundEngine.SetRTPCValue("SFXVolume", .4f);
        }
    }
    
    public void SaveScreenShot(int i)
    {
        var sS = ScaleTexture(ScreenCapture.CaptureScreenshotAsTexture(), 256,256);
        byte[] textureBytes = sS.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + "ScreenShot" + i + ".png", textureBytes);
    }
    private Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) 
    {
        Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
        Color[] rpixels=result.GetPixels(0);
        float incX=(1.0f / (float)targetWidth);
        float incY=(1.0f / (float)targetHeight); 
        for(int px=0; px<rpixels.Length; px++) { 
            rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth), incY*((float)Mathf.Floor(px/targetWidth))); 
        } 
        result.SetPixels(rpixels,0); 
        result.Apply(); 
        return result; 
    }
    public Texture2D LoadTextureFromDisk(int i)
    {
        byte[] textureBytes =
            File.ReadAllBytes(Application.persistentDataPath + "ScreenShot" + i + ".png");
        var sS = new Texture2D(0, 0);
        sS.LoadImage(textureBytes);
        sS.filterMode = FilterMode.Point;
        return sS;
    }

    public void HandleSaving(int gameNumber)
    {
        SaveGameEvent?.Invoke(null, EventArgs.Empty);
        SaveGameData.deliveryPackages = new List<DeliveryPackage>();
        foreach (var d in _gameMode.DeliveryManager.GetQueue())
        {
            foreach (var dP in d.GetDeliveryPackages())
            {
                SaveGameData.deliveryPackages.Add(dP);
            }
        }
        SaveGameData.playerMoney = _gameModeData.moneyInBank;
        var tooEarly = _gameModeData.currentTime.Hour is >= 0 and < 6;
        var cT = _gameModeData.currentTime;
        SaveGameData.savedHour = tooEarly? 6 : _gameModeData.currentTime.Hour;
        SaveGameData.savedDay = _gameModeData.currentTime.Day;
        SaveGameData.savedMonth =_gameModeData.currentTime.Month;
        SaveGameData.savedYear =_gameModeData.currentTime.Year;
        var json = JsonUtility.ToJson(SaveGameData);
        SaveGameToDisk(gameNumber,"SaveGame",json);  
    }
    public void SaveGameToDisk(int gameNumber,string fileName,string json)
    {
        using (StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath +fileName+gameNumber+".json"))
        {
            streamWriter.Write(json);
        }
    }

    public void HandleLoading(int gameNumber)
    {
        
        if (File.Exists(Application.persistentDataPath +$"SaveGame{gameNumber}.json"))
        {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath +$"SaveGame{gameNumber}.json"))
            {
                var json = streamReader.ReadToEnd();
                SaveGameData = JsonUtility.FromJson<SaveGameData>(json);
            }

            var saveDate = new DateTime(SaveGameData.savedYear, SaveGameData.savedMonth, SaveGameData.savedDay, 
                SaveGameData.savedHour, 0, 0);
                _gameModeData.currentTime = saveDate;
                _gameModeData.moneyInBank = SaveGameData.playerMoney;
                _gameMode.DeliveryManager = new DeliveryManager(_gameMode.DeliveryManager, _gameMode, _gameModeData);
                foreach (var dP in SaveGameData.deliveryPackages)
                {
                    _gameMode.DeliveryManager.AddToDelivery(dP);
                }
                _gameMode.SpawnSavedObjects();
        }
        else
        {
                _gameModeData.currentTime = new DateTime(2027, 1, 1, 6, 0,0);
                _gameMode.DeliveryManager = new DeliveryManager(_gameMode.DeliveryManager, _gameMode, _gameModeData);
                SaveGameData = new SaveGameData
                {
                    playerMoney = _gameModeData.moneyInBank,
                    deliveryPackages = new List<DeliveryPackage>(),
                    respawnables = new List<RespawbableData>()
                };
        }
    }
}
