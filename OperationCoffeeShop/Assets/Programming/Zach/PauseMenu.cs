using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PauseMenu : MonoBehaviour
{
    private GameMode gM;
    [SerializeField]private string quitScene;
    private PlayerInteraction pI;
    public PlayerData pD;
    
    
    public GameObject optionsScreen;

    private Animator animator;

    //Bellow is all of the functions for managing what buttons do in the main menu.
    public void StartGame()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", gameObject);
        animator.SetTrigger("Reverse");//
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        gM.pD.canMove = true;
        pD.canMove = true;
        pD.neckClamp = 77.3f;
        pD.inUI = false;
        this.gameObject.SetActive(false);
        pI.CameraBlur();
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
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        animator = GetComponent<Animator>();
        pI =gM.player.gameObject.GetComponent<PlayerInteraction>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gM.pD.canMove = false;
        pD.canMove = false;
        pD.neckClamp = 0.0f;
        pI.CameraBlur();
    }
    

    private void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        animator = GetComponent<Animator>();
        pI =gM.player.gameObject.GetComponent<PlayerInteraction>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gM.pD.canMove = false;
        pD.canMove = false;
        pD.neckClamp = 0.0f;
        pI.CameraBlur();
    }
}
