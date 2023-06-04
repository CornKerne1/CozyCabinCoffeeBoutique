using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
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

    public TMP_Text resolutionLabel,performanceLabel,fullscreenLabel,vsyncLabel,mastLabel, musicLabel, sfxLabel,mouseLabel,renderLabel,fovLabel;
    private bool _playing;

    public UniversalRenderPipelineAsset urpA;

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
        if (!gM.SaveSystem.SaveOptionsData.performanceMode)
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
        gM.SaveSystem.SaveOptionsData.performanceMode = false;
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
        gM.SaveSystem.SaveOptionsData.performanceMode = true;
        urpA.msaaSampleCount = 0;
        urpA.supportsCameraDepthTexture = false;
        urpA.supportsCameraOpaqueTexture = false;
        urpA.shadowCascadeCount = 1;
        urpA.supportsHDR = false;
        urpA.shadowDistance = 45;
        performanceLabel.text = "ON";
    }
    async void Start()
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

        await Task.Delay(50);
        mouseLabel.text = ((int)(gM.playerData.mouseSensitivityOptions * 10)).ToString();
        mastLabel.text = Mathf.RoundToInt(gM.SaveSystem.SaveOptionsData.masterVol).ToString();
        musicLabel.text = Mathf.RoundToInt(gM.SaveSystem.SaveOptionsData.musicVol).ToString();
        sfxLabel.text = Mathf.RoundToInt(gM.SaveSystem.SaveOptionsData.sfxVol).ToString();
        renderLabel.text = gM.SaveSystem.SaveOptionsData.renderScale.ToString();
        fovLabel.text = Mathf.RoundToInt(gM.SaveSystem.SaveOptionsData.fov).ToString();
        TogglePerformance( gM.SaveSystem.SaveOptionsData.performanceMode);
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
    public void RenderScale(string dir)
    {
        gM.SaveSystem.SaveOptionsData.renderScale = dir == "left" ? Mathf.Clamp(gM.SaveSystem.SaveOptionsData.renderScale-.05f,0f,2f) :  Mathf.Clamp(gM.SaveSystem.SaveOptionsData.renderScale+.05f,0f,2f);
        StartCoroutine(CO_PlayAudioWWisely());
        renderLabel.text = gM.SaveSystem.SaveOptionsData.renderScale.ToString();
        gM.SaveSystem.SaveOptionsData.renderScale = gM.SaveSystem.SaveOptionsData.renderScale;
        urpA.renderScale = gM.SaveSystem.SaveOptionsData.renderScale;
        Save(0);
    }
    public void CameraFov(string dir)
    {
        
        gM.SaveSystem.SaveOptionsData.fov = dir == "left" ? Mathf.Clamp(gM.SaveSystem.SaveOptionsData.fov-1,60f,100f) :  Mathf.Clamp(gM.SaveSystem.SaveOptionsData.fov+1f,60f,100f);
        StartCoroutine(CO_PlayAudioWWisely());
        fovLabel.text = Mathf.RoundToInt(gM.SaveSystem.SaveOptionsData.fov).ToString();
        gM.SaveSystem.SaveOptionsData.fov = gM.SaveSystem.SaveOptionsData.fov;
        gM.UpdatePlayerCameraFov();
        Save(0);
    }

    //functions below control sound slider values and how they interact with ui
    public void MasterVol(string dir)
    {
        gM.SaveSystem.SaveOptionsData.masterVol = dir == "left" ? gM.SaveSystem.SaveOptionsData.masterVol - 5 : gM.SaveSystem.SaveOptionsData.masterVol + 5;
        if (gM.SaveSystem.SaveOptionsData.masterVol < 0) gM.SaveSystem.SaveOptionsData.masterVol = 0;
        if (gM.SaveSystem.SaveOptionsData.masterVol > 100) gM.SaveSystem.SaveOptionsData.masterVol = 100;
        StartCoroutine(CO_PlayAudioWWisely());
        mastLabel.text = Mathf.RoundToInt(gM.SaveSystem.SaveOptionsData.masterVol) .ToString();
        gM.SaveSystem.SaveOptionsData.masterVol = gM.SaveSystem.SaveOptionsData.masterVol;
        AkSoundEngine.SetRTPCValue("MasterVolume", gM.SaveSystem.SaveOptionsData.masterVol);
        Save(0);
    }
    public void MusicVol(string dir)
    {
        gM.SaveSystem.SaveOptionsData.musicVol = dir == "left" ? gM.SaveSystem.SaveOptionsData.musicVol - 5 : gM.SaveSystem.SaveOptionsData.musicVol + 5;
        if (gM.SaveSystem.SaveOptionsData.musicVol < 0) gM.SaveSystem.SaveOptionsData.musicVol = 0;
        if (gM.SaveSystem.SaveOptionsData.musicVol > 100) gM.SaveSystem.SaveOptionsData.musicVol = 100;
        StartCoroutine(CO_PlayAudioWWisely());
        musicLabel.text = Mathf.RoundToInt(gM.SaveSystem.SaveOptionsData.musicVol).ToString();
        gM.SaveSystem.SaveOptionsData.musicVol = gM.SaveSystem.SaveOptionsData.musicVol;
        AkSoundEngine.SetRTPCValue("MusicVolume", gM.SaveSystem.SaveOptionsData.musicVol);
        Save(0);
    }
    public void SFXVol(string dir)
    {
        gM.SaveSystem.SaveOptionsData.sfxVol = dir == "left" ? gM.SaveSystem.SaveOptionsData.sfxVol - 5 : gM.SaveSystem.SaveOptionsData.sfxVol + 5;
        if (gM.SaveSystem.SaveOptionsData.sfxVol < 0) gM.SaveSystem.SaveOptionsData.sfxVol = 0;
        if (gM.SaveSystem.SaveOptionsData.sfxVol > 100) gM.SaveSystem.SaveOptionsData.sfxVol = 100;
        StartCoroutine(CO_PlayAudioWWisely());
        sfxLabel.text = Mathf.RoundToInt(gM.SaveSystem.SaveOptionsData.sfxVol).ToString();
        gM.SaveSystem.SaveOptionsData.sfxVol = gM.SaveSystem.SaveOptionsData.sfxVol;
        AkSoundEngine.SetRTPCValue("SFXVolume", gM.SaveSystem.SaveOptionsData.sfxVol);
        Save(0);
    }
    public void SetMouse(string dir)
    {
        gM.playerData.mouseSensitivityOptions = dir == "left" ? gM.playerData.mouseSensitivityOptions - .03f : gM.playerData.mouseSensitivityOptions + .03f;
        if (gM.playerData.mouseSensitivityOptions < 0f) gM.playerData.mouseSensitivityOptions = 0f;
        if (gM.playerData.mouseSensitivityOptions > 1f) gM.playerData.mouseSensitivityOptions = 1f;
        StartCoroutine(CO_PlayAudioWWisely());
        gM.playerData.mouseSensitivityOptions = gM.playerData.mouseSensitivityOptions;
        mouseLabel.text = ((int)(gM.playerData.mouseSensitivityOptions * 100)).ToString();
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
        if (File.Exists(Application.persistentDataPath +$"SaveOptions{gameNumber}.json"))
        {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath + $"SaveOptions{gameNumber}.json"))
            {
                var json = streamReader.ReadToEnd();
                gM.SaveSystem.SaveOptionsData = JsonUtility.FromJson<SaveOptionsData>(json);
            }
        }
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
