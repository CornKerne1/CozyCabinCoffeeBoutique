using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PicturePickerUI : MonoBehaviour
{
    public PictureFrame physicalRef;
    public GameMode gameMode;

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }
    public void OnLeftButtonClicked()
    {
        if (physicalRef.currentPic == 0) return;
        if (File.Exists(Application.persistentDataPath + "ScreenShot" + (physicalRef.currentPic - 1) + ".png"))
        {
            physicalRef.currentPic = physicalRef.currentPic - 1;
            physicalRef.ChangePicture(GameMode.LoadTextureFromDisk(physicalRef.currentPic));
        }

        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
    }
    public void OnRightButtonClicked()
    {
       
        if(File.Exists(Application.persistentDataPath + "ScreenShot" + (physicalRef.currentPic+1) + ".png"))
        {
            physicalRef.currentPic = physicalRef.currentPic + 1;
            physicalRef.ChangePicture(GameMode.LoadTextureFromDisk(physicalRef.currentPic));
        }
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
    }

    public void OnCloseClicked()
    {
        physicalRef.DestroyUI();
    }
}
