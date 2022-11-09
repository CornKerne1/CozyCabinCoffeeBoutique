using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PicturePickerUI : MonoBehaviour
{
    public PictureFrame physicalRef;
    public GameMode gameMode;
    private int _currentPic;

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }
    public void OnLeftButtonClicked()
    {
        if (_currentPic == 0) return;
        if (File.Exists(Application.persistentDataPath + "ScreenShot" + (_currentPic - 1) + ".png"))
        {
            _currentPic = _currentPic - 1;
            physicalRef.ChangePicture(GameMode.LoadTextureFromDisk(_currentPic));
        }

        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
    }
    public void OnRightButtonClicked()
    {
       
        if(File.Exists(Application.persistentDataPath + "ScreenShot" + (_currentPic+1) + ".png"))
        {
            _currentPic = _currentPic + 1;
            physicalRef.ChangePicture(GameMode.LoadTextureFromDisk(_currentPic));
        }
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
    }
}
