using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    private GameMode _gameMode;
    [SerializeField] private string quitScene;
    private PlayerInteraction _playerInteraction;
    public PlayerData pD;
    private float _previousTimeRate;
    private bool _couldMovePreviously = true;
    public PlayerInput playerInput;

    [SerializeField]private GameObject infoScreen,optionsScreen,controlScreen,infoButtonObj, optionsButtonObj,controlsButtonObj,wwiseBank;
    private Image _infoButton, _optionsButton,_controlsButton;
    [SerializeField] private Color activateColor, deactivateColor;

    private Animator _animator;

    public void StartGame()
    {
        if (_couldMovePreviously)
        {
            Debug.Log("using StartGame() _couldMovePreviously is true");

            _gameMode.gameModeData.timeRate = _previousTimeRate;
            AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
            _animator.SetTrigger("Reverse");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerInput.ToggleMovement();
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
            playerInput.ToggleMovement();
        }
    }

    public void ChangeTab(string tabName)
    {
        switch (tabName)
        {
            case "info":
                optionsScreen.SetActive(false);
                controlScreen.SetActive(false);
                infoScreen.SetActive(true);
                _optionsButton.color = deactivateColor;
                _controlsButton.color =deactivateColor;
                _infoButton.color = activateColor;
                break;
            case "options":
                infoScreen.SetActive(false);
                controlScreen.SetActive(false);
                optionsScreen.SetActive(true);
                _infoButton.color = deactivateColor;
                _controlsButton.color =deactivateColor;
                _optionsButton.color = activateColor;
                break;
            case "controls":
                infoScreen.SetActive(false);
                optionsScreen.SetActive(false);
                controlScreen.SetActive(true);
                _infoButton.color = deactivateColor;
                _optionsButton.color =deactivateColor;
                _controlsButton.color = activateColor;
                break;
            case "close":
                StartGame();
                break;
            case "quit":
                QuitGame();
                break;
        }
    }

    public void OpenOptions()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        optionsScreen.SetActive(!optionsScreen.activeSelf);
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
        Destroy(wwiseBank);
        StartCoroutine(WaitForSave());
    }

    private IEnumerator WaitForSave()
    {
        yield return new WaitForSeconds(.04f);
        Application.Quit();
        //EditorApplication.ExitPlaymode();
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
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        playerInput.ToggleMovement();
        _playerInteraction.CameraBlur();
        _infoButton = infoButtonObj.GetComponent<UnityEngine.UI.Image>();
        _optionsButton = optionsButtonObj.GetComponent<UnityEngine.UI.Image>();
        _controlsButton = controlsButtonObj.GetComponent<UnityEngine.UI.Image>();
        ChangeTab("info");
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