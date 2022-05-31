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
    private PlayerMovement pM;
    private PlayerCameraController pCC;
    private PlayerInteraction pI;
    public PlayerData pD;
    
    
    public GameObject optionsScreen;

    private Animator animator;

    //Bellow is all of the functions for managing what buttons do in the main menu.
    public void StartGame()
    {
        animator.SetTrigger("Reverse");//
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        pM.canMove = true;
        pCC.canMove = true;
        pI.CameraBlur();
        pD.neckClamp = 77.3f;
        pD.inUI = false;
        this.gameObject.SetActive(false);
    }

    public void OpenOptions()
    {
        Instantiate(optionsScreen, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void CloseOptions()
    {
        pD.inUI = false;
        this.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        pD.inUI = false;
        SceneManager.LoadScene(quitScene);
    }

    private void OnEnable()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        animator = GetComponent<Animator>();
        pM = gM.player.gameObject.GetComponent<PlayerMovement>();
        pCC = gM.player.gameObject.GetComponent<PlayerCameraController>();
        pI =gM.player.gameObject.GetComponent<PlayerInteraction>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pI.CameraBlur();
        pM.canMove = false;
        pCC.canMove = false;
        pD.neckClamp = 0.0f;
    }
    

    private void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        animator = GetComponent<Animator>();
        pM = gM.player.gameObject.GetComponent<PlayerMovement>();
        pCC = gM.player.gameObject.GetComponent<PlayerCameraController>();
        pI =gM.player.gameObject.GetComponent<PlayerInteraction>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pI.CameraBlur();
        pM.canMove = false;
        pCC.canMove = false;
        pD.neckClamp = 0.0f;
    }
}
