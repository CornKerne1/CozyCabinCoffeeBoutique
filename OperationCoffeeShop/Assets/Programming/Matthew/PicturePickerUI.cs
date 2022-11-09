using System;
using System.Collections;
using System.Collections.Generic;
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

        int pic;
        if (_currentPic == 0)
            pic = gameMode.gameModeData.screenShots.Count-1;
        else
            pic = _currentPic - 1;
        _currentPic = pic;
        physicalRef.ChangePicture(_currentPic);
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
    }
    public void OnRightButtonClicked()
    {
        int pic;
        if (_currentPic == gameMode.gameModeData.screenShots.Count-1)
            pic = 0;
        else
            pic = _currentPic + 1;
        _currentPic = pic;
        physicalRef.ChangePicture(_currentPic);
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
    }
}
