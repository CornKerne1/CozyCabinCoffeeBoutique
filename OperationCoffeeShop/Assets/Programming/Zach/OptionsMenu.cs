using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
public class OptionsMenu : MonoBehaviour, ISaveState
{
    public GameObject optionsScreen;
    
    public GameMode gM;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel,performanceLabel,fullscreenLabel;
    private bool playing;
    

    public TMP_Text mastLabel, musicLabel, sfxLabel;
    public Slider masterSlider, musicSlider, sfxSlider, mouseSlider;

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

                selectedResolution = 1;

                UpdateResLabel(); 
            }
        }

        if(!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;
            UpdateResLabel();
        }

        float vol = 0f;
        mastLabel.text = Mathf.RoundToInt(masterSlider.value).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value).ToString();
        TogglePerformance(_lowQualityMode);
    }

    public void ResLeft()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    //functions below control sound slider values and how they interact with ui
    public void SetMasterVol()
    {
        StartCoroutine(CO_PlayAudioWWisely());
        mastLabel.text = Mathf.RoundToInt(masterSlider.value) .ToString();
        gM.SaveSystem.SaveOptionsData.masterVol = masterSlider.value;
        AkSoundEngine.SetRTPCValue("MasterVolume", gM.SaveSystem.SaveOptionsData.masterVol);
        gM.SaveSystem.SaveGameToDisk(0,"SaveOptions",JsonUtility.ToJson(gM.SaveSystem.SaveOptionsData));

    }
    public void SetMusicVol()
    {
        StartCoroutine(CO_PlayAudioWWisely());
        musicLabel.text = Mathf.RoundToInt(musicSlider.value).ToString();
        gM.SaveSystem.SaveOptionsData.musicVol = musicSlider.value;
        AkSoundEngine.SetRTPCValue("MusicVolume", gM.SaveSystem.SaveOptionsData.musicVol);
        gM.SaveSystem.SaveGameToDisk(0,"SaveOptions",JsonUtility.ToJson(gM.SaveSystem.SaveOptionsData));

    }
    public void SetSFXVol()
    {
        StartCoroutine(CO_PlayAudioWWisely());
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value).ToString();
        gM.SaveSystem.SaveOptionsData.sfxVol = sfxSlider.value;
        AkSoundEngine.SetRTPCValue("SFXVolume", gM.SaveSystem.SaveOptionsData.sfxVol);
        gM.SaveSystem.SaveGameToDisk(0,"SaveOptions",JsonUtility.ToJson(gM.SaveSystem.SaveOptionsData));
        
    }
    public void SetMouse()
    {
        StartCoroutine(CO_PlayAudioWWisely());
        gM.playerData.mouseSensitivityOptions = mouseSlider.value;
    }
    public void CloseOptions()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        Destroy(optionsScreen);
    }

    private IEnumerator CO_PlayAudioWWisely()
    {
        if (playing)
            yield break;
        playing = true;
        yield return new WaitForSeconds(.1f);
        playing = false;
        AkSoundEngine.PostEvent("Play_Slider", gameObject);
    }

    public void Save(int gameNumber)
    {
        gM.SaveSystem.SaveGameToDisk(0,"SaveOptions",JsonUtility.ToJson(gM.SaveSystem.SaveOptionsData));
    }

    public void Load(int gameNumber)
    {
        if (File.Exists(Application.persistentDataPath +$"SaveOptions{gameNumber}.json"))
        {
            using (StreamReader streamReader = new StreamReader(Application.persistentDataPath +$"SaveOptions{gameNumber}.json"))
            {
                var json = streamReader.ReadToEnd();
                gM.SaveSystem.SaveOptionsData = JsonUtility.FromJson<SaveOptionsData>(json);
            }

            _lowQualityMode = gM.SaveSystem.SaveOptionsData.performanceMode;
            mouseSlider.value = gM.playerData.mouseSensitivityOptions;
            masterSlider.value = gM.SaveSystem.SaveOptionsData.masterVol;
            musicSlider.value = gM.SaveSystem.SaveOptionsData.musicVol;
            sfxSlider.value = gM.SaveSystem.SaveOptionsData.sfxVol;
        }
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
