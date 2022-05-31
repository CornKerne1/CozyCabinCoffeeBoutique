using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
public class OptionsMenu : MonoBehaviour
{
    public ScriptableOptions sO;
    public GameObject optionsScreen;
    

    public Toggle fullscreenTog, vsyncTog;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel;

    public AudioMixer theMixer;

    public TMP_Text mastLabel, musicLabel, sfxLabel;
    public Slider masterSlider, musicSlider, sfxSlider;

    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

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
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
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
        //Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

    //functions below control sound slider values and how they interact with ui
    public void SetMasterVol()
    {
        mastLabel.text = Mathf.RoundToInt(masterSlider.value) .ToString();
        sO.MasterVol = masterSlider.value;
        AkSoundEngine.SetRTPCValue("MasterVolume", sO.MasterVol);
        
    }
    public void SetMusicVol()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value).ToString();
        sO.MusicVol = musicSlider.value;
        AkSoundEngine.SetRTPCValue("MusicVolume", sO.MusicVol);

    }
    public void SetSFXVol()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value).ToString();
        sO.SFXVol = sfxSlider.value;
        AkSoundEngine.SetRTPCValue("SFXVolume", sO.SFXVol);
        
    }
    public void CloseOptions()
    {
       Destroy(optionsScreen);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
