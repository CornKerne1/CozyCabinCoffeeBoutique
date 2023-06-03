using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
public class OptionsMenu : MonoBehaviour, ISaveState
{
    public GameObject optionsScreen;
    
    public GameMode gM;

    public List<ResItem> resolutions = new List<ResItem>();
    private int _selectedResolution;

    public TMP_Text resolutionLabel,performanceLabel,fullscreenLabel,vsyncLabel,mastLabel, musicLabel, sfxLabel,mouseLabel;
    private bool _playing;
    private float _masterVol, _musicVol, _sfxVol,_mouseVal;

    public UniversalRenderPipelineAsset urpA;
    private bool _lowQualityMode;

    private void OnDisable() => Save(0);

    public void TogglePerformance(bool lowQualitySetting)
    {
        if (lowQualitySetting)
            LowQualityMode();
        else
            HighQualityMode();
    }
    public void TogglePerformance()
    {
        if (!_lowQualityMode)
            LowQualityMode();
        else
            HighQualityMode();
    }
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        fullscreenLabel.text = Screen.fullScreen.ToString();
    }

    public void ToggleVSync()
    {
        if (Application.targetFrameRate == -1)
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;
            vsyncLabel.text = "ON";
        }
        else
        {
            Application.targetFrameRate = -1;
            QualitySettings.vSyncCount = 0;
            vsyncLabel.text = "OFF";
        }
    }
    private void HighQualityMode()
    {
        _lowQualityMode = false;
        urpA.msaaSampleCount = 8;
        urpA.supportsCameraDepthTexture = true;
        urpA.supportsCameraOpaqueTexture = true;
        urpA.shadowCascadeCount = 3;
        urpA.supportsHDR = true;
        urpA.shadowDistance = 180;
        performanceLabel.text = "OFF";
    }

    private void LowQualityMode()
    {
        _lowQualityMode = true;
        urpA.msaaSampleCount = 0;
        urpA.supportsCameraDepthTexture = false;
        urpA.supportsCameraOpaqueTexture = false;
        urpA.shadowCascadeCount = 1;
        urpA.supportsHDR = false;
        urpA.shadowDistance = 45;
        performanceLabel.text = "ON";
    }
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        Load(0);
        bool foundRes = false;
        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                _selectedResolution = 1;

                UpdateResLabel(); 
            }
        }

        if(!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            _selectedResolution = resolutions.Count - 1;
            UpdateResLabel();
        }

        float vol = 0f;
        mouseLabel.text = ((int)(_mouseVal * 10)).ToString();
        mastLabel.text = Mathf.RoundToInt(_masterVol).ToString();
        musicLabel.text = Mathf.RoundToInt(_musicVol).ToString();
        sfxLabel.text = Mathf.RoundToInt(_sfxVol).ToString();
        TogglePerformance(_lowQualityMode);
    }

    public void ResLeft()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        _selectedResolution--;
        if(_selectedResolution < 0)
        {
            _selectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        _selectedResolution++;
        if (_selectedResolution > resolutions.Count - 1)
        {
            _selectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[_selectedResolution].horizontal.ToString() + " x " + resolutions[_selectedResolution].vertical.ToString();
    }

    //functions below control sound slider values and how they interact with ui
    public void MasterVol(string dir)
    {
        _masterVol = dir == "left" ? _masterVol - 5 : _masterVol + 5;
        if (_masterVol < 0) _masterVol = 0;
        if (_masterVol > 100) _masterVol = 100;
        StartCoroutine(CO_PlayAudioWWisely());
        mastLabel.text = Mathf.RoundToInt(_masterVol) .ToString();
        gM.SaveSystem.SaveOptionsData.masterVol = _masterVol;
        AkSoundEngine.SetRTPCValue("MasterVolume", gM.SaveSystem.SaveOptionsData.masterVol);
        Save(0);
    }
    public void MusicVol(string dir)
    {
        _musicVol = dir == "left" ? _musicVol - 5 : _musicVol + 5;
        if (_musicVol < 0) _musicVol = 0;
        if (_musicVol > 100) _musicVol = 100;
        StartCoroutine(CO_PlayAudioWWisely());
        musicLabel.text = Mathf.RoundToInt(_musicVol).ToString();
        gM.SaveSystem.SaveOptionsData.musicVol = _musicVol;
        AkSoundEngine.SetRTPCValue("MusicVolume", gM.SaveSystem.SaveOptionsData.musicVol);
        Save(0);
    }
    public void SFXVol(string dir)
    {
        _sfxVol = dir == "left" ? _sfxVol - 5 : _sfxVol + 5;
        if (_sfxVol < 0) _sfxVol = 0;
        if (_sfxVol > 100) _sfxVol = 100;
        StartCoroutine(CO_PlayAudioWWisely());
        sfxLabel.text = Mathf.RoundToInt(_sfxVol).ToString();
        gM.SaveSystem.SaveOptionsData.sfxVol = _sfxVol;
        AkSoundEngine.SetRTPCValue("SFXVolume", gM.SaveSystem.SaveOptionsData.sfxVol);
        Save(0);
    }
    public void SetMouse(string dir)
    {
        _mouseVal = dir == "left" ? _mouseVal - .03f : _mouseVal + .03f;
        if (_mouseVal < 0f) _mouseVal = 0f;
        if (_mouseVal > 1f) _mouseVal = 1f;
        StartCoroutine(CO_PlayAudioWWisely());
        gM.playerData.mouseSensitivityOptions = _mouseVal;
        mouseLabel.text = ((int)(_mouseVal * 100)).ToString();
    }
    public void CloseOptions()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        Destroy(optionsScreen);
    }

    private IEnumerator CO_PlayAudioWWisely()
    {
        if (_playing)
            yield break;
        _playing = true;
        yield return new WaitForSeconds(.1f);
        _playing = false;
        AkSoundEngine.PostEvent("Play_Slider", gameObject);
    }

    public void Save(int gameNumber)
    {
        var json=JsonUtility.ToJson(gM.SaveSystem.SaveOptionsData);
        gM.SaveSystem.SaveGameToDisk(0,"SaveOptions",json);
    }

    public void Load(int gameNumber)
    {
        if (File.Exists(Application.persistentDataPath +$"SaveOptions{gameNumber}.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + $"SaveOptions{gameNumber}.sav",FileMode.Open);
            gM.SaveSystem.SaveOptionsData=bf.Deserialize(stream) as SaveOptionsData;
            stream.Close();
            if (gM.SaveSystem.SaveOptionsData != null)
            {
                _lowQualityMode = gM.SaveSystem.SaveOptionsData.performanceMode;
                _mouseVal = gM.playerData.mouseSensitivityOptions;
                _masterVol = gM.SaveSystem.SaveOptionsData.masterVol;
                _musicVol = gM.SaveSystem.SaveOptionsData.musicVol;
                _sfxVol = gM.SaveSystem.SaveOptionsData.sfxVol;
            }

            if (_masterVol != 0) return;
            _masterVol = 50;
            _musicVol = 50;
            _sfxVol = 50;
        }
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
