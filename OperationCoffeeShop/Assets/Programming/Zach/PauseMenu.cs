using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    private GameMode _gameMode;
    [SerializeField] private string quitScene;
    private PlayerInteraction _playerInteraction;
    public PlayerData pD;
    private float _previousTimeRate;
    private bool _couldMovePreviously = true;
    public PlayerInput playerInput;

    public GameObject optionsScreen;

    private Animator _animator;

    public void StartGame()
    {
        if (_couldMovePreviously)
        {
            Debug.Log("using StartGame() _couldMovePreviously is true");

            _gameMode.gameModeData.timeRate = _previousTimeRate;
            AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
            _animator.SetTrigger("Reverse");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            _gameMode.playerData.canMove = true;
            pD.canMove = true;
            pD.neckClamp = 77.3f;
            pD.inUI = false;
            gameObject.SetActive(false);
            _playerInteraction.CameraBlur();
            playerInput.ToggleHud();
        }
        else
        {
            _couldMovePreviously = true;
            _gameMode.gameModeData.timeRate = _previousTimeRate;
            AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
            _animator.SetTrigger("Reverse");
            gameObject.SetActive(false);
            playerInput.ToggleHud();
        }
    }


    public void OpenOptions()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        Instantiate(optionsScreen, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void CloseOptions()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        pD.inUI = false;
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        if (_playerInteraction.playerData.camMode)
            _playerInteraction.carriedObj.SetActive(true);
        _gameMode.Save(0);
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        StartCoroutine(WaitForSave());
    }

    private IEnumerator WaitForSave()
    {
        yield return new WaitForSeconds(.04f);
        Application.Quit();
        EditorApplication.ExitPlaymode();
    }

    private void OnEnable()
    {
        Debug.Log("using OnEnable");

        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _previousTimeRate = _gameMode.gameModeData.timeRate;
        _gameMode.gameModeData.timeRate = 0;
        Debug.Log("_game mode time rate " + _gameMode.gameModeData.timeRate);
        Debug.Log("previous time rate " + _previousTimeRate);

        if (!pD.canMove)
        {
            Debug.Log("using OnEnable _couldMove Previously is false");
            _couldMovePreviously = false;
        }

        _animator = GetComponent<Animator>();
        _playerInteraction = _gameMode.player.gameObject.GetComponent<PlayerInteraction>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _gameMode.playerData.canMove = false;
        pD.canMove = false;
        pD.neckClamp = 0.0f;
        _playerInteraction.CameraBlur();
    }


    /* private void Start()
     {
         _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
         _previousTimeRate = _gameMode.gameModeData.timeRate;
         _gameMode.gameModeData.timeRate = 0;
 
         if (! pD.canMove)
         {
             Debug.Log("using start _couldMove Previously is false");
             _couldMovePreviously = false;
         }
 
         _animator = GetComponent<Animator>();
         _playerInteraction = _gameMode.player.gameObject.GetComponent<PlayerInteraction>();
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
         _gameMode.playerData.canMove = false;
         pD.canMove = false;
         pD.neckClamp = 0.0f;
         _playerInteraction.CameraBlur();
     }*/
}