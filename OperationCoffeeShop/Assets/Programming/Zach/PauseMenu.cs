using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    private GameMode _gameMode;
    [SerializeField] private string quitScene;
    private PlayerInteraction _playerInteraction;
    public PlayerData pD;


    public GameObject optionsScreen;

    private Animator _animator;

    public void StartGame()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        _animator.SetTrigger("Reverse"); 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        _gameMode.pD.canMove = true;
        pD.canMove = true;
        pD.neckClamp = 77.3f;
        pD.inUI = false;
        gameObject.SetActive(false);
        _playerInteraction.CameraBlur();
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
        this.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        Application.Quit();
    }

    private void OnEnable()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _animator = GetComponent<Animator>();
        _playerInteraction = _gameMode.player.gameObject.GetComponent<PlayerInteraction>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _gameMode.pD.canMove = false;
        pD.canMove = false;
        pD.neckClamp = 0.0f;
        _playerInteraction.CameraBlur();
    }


    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _animator = GetComponent<Animator>();
        _playerInteraction = _gameMode.player.gameObject.GetComponent<PlayerInteraction>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _gameMode.pD.canMove = false;
        pD.canMove = false;
        pD.neckClamp = 0.0f;
        _playerInteraction.CameraBlur();
    }
}