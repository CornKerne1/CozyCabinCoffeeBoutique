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
        PlayerInput.AltInteractEvent += Close;
    }

    private void Close(object sender, EventArgs e)
    {
        gameMode.playerData.canMove = true;
        gameMode.playerData.inUI = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(gameObject.transform.root.gameObject);
    }

    public void OnLeftButtonClicked()
    {

        int pic;
        if (_currentPic == 0)
            pic = gameMode.playerData.screenShots.Count-1;
        else
            pic = _currentPic - 1;
        _currentPic = pic;
        physicalRef.ChangePicture(_currentPic);
    }
    public void OnRightButtonClicked()
    {
        int pic;
        if (_currentPic == gameMode.playerData.screenShots.Count)
            pic = 0;
        else
            pic = _currentPic + 1;
        _currentPic = pic;
        physicalRef.ChangePicture(_currentPic);
    }
}
