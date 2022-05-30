using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class MainMenu : MonoBehaviour
{
    public string scene;
    
    public GameObject optionsScreen;

    [SerializeField] PlayableDirector director;

    private Animator animator;

    //Bellow is all of the functions for managing what buttons do in the main menu.
    public void StartGame()
    {
        animator.SetTrigger("Reverse");
        director.Play();
    }
    private void LaunchGame(PlayableDirector aDirector)
    {
        SceneManager.LoadScene(scene);
        AkSoundEngine.PostEvent("Stop_TitleTheme", this.gameObject);
    }

    public void OpenOptions()
    {
        Instantiate(optionsScreen, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }

    private void Start()
    {
        AkSoundEngine.PostEvent("Play_TitleTheme", this.gameObject);
        animator = GetComponent<Animator>();
        GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().gMD.isOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        director.stopped += LaunchGame;

    }
}
