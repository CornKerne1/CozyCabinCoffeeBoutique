using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
public class OptionsMenu : MonoBehaviour
{
    [SerializeField]private ScriptableOptions sO;
    public GameObject optionsScreen;
    
    public GameMode gM;

    public Toggle fullscreenTog, vsyncTog, performanceTog;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel;
    private bool playing;
    

    public TMP_Text mastLabel, musicLabel, sfxLabel;
    public Slider masterSlider, musicSlider, sfxSlider, mouseSlider;

    public UniversalRenderPipelineAsset urpA;

    public void TogglePerformace(bool lowQuality)
    {
        if (lowQuality)
        {
            urpA.msaaSampleCount = 0;
            urpA.supportsCameraDepthTexture = false;
            urpA.supportsCameraOpaqueTexture = false;
            urpA.shadowCascadeCount = 1;
            urpA.supportsHDR = false;
            urpA.shadowDistance = 45;
        }
        else
        {
            urpA.msaaSampleCount = 8;
            urpA.supportsCameraDepthTexture = true;
            urpA.supportsCameraOpaqueTexture = true;
            urpA.shadowCascadeCount = 3;
            urpA.supportsHDR = true;
            urpA.shadowDistance = 180;
        }
    }
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;
        performanceTog.isOn = false;
        TogglePerformace(performanceTog.isOn);

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        } else
        {
            vsyncTog.isOn = true;
        }
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
        masterSlider.value = sO.MasterVol;
        
        musicSlider.value = sO.MusicVol;
        
        sfxSlider.value = sO.SFXVol;
        
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        mouseSlider.value = gM.playerData.mouseSensitivityOptions;

        mastLabel.text = Mathf.RoundToInt(masterSlider.value).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenTog.isOn;
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
        TogglePerformace(performanceTog.isOn);
    }

    //functions below control sound slider values and how they interact with ui
    public void SetMasterVol()
    {
        StartCoroutine(CO_PlayAudioWWisely());
        mastLabel.text = Mathf.RoundToInt(masterSlider.value) .ToString();
        sO.MasterVol = masterSlider.value;
        AkSoundEngine.SetRTPCValue("MasterVolume", sO.MasterVol);

    }
    public void SetMusicVol()
    {
        StartCoroutine(CO_PlayAudioWWisely());
        musicLabel.text = Mathf.RoundToInt(musicSlider.value).ToString();
        sO.MusicVol = musicSlider.value;
        AkSoundEngine.SetRTPCValue("MusicVolume", sO.MusicVol);

    }
    public void SetSFXVol()
    {
        StartCoroutine(CO_PlayAudioWWisely());
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value).ToString();
        sO.SFXVol = sfxSlider.value;
        AkSoundEngine.SetRTPCValue("SFXVolume", sO.SFXVol);
        
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
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
