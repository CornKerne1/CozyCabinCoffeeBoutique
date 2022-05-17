using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string scene;
    
    public GameObject optionsScreen;

    //Bellow is all of the functions for managing what buttons do in the main menu.
    public void StartGame()
    {
        SceneManager.LoadScene(scene);
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
        GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().gMD.isOpen = true;
    }
}
