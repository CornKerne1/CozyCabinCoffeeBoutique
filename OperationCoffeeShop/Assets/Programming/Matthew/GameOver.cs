using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private GameMode gM;

    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject winScreen;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.Find("GameMode").GetComponent<GameMode>();
        if (gM.gMD.reputation < 6)
        {
            loseScreen.SetActive(true);
        }
        else
        {
            winScreen.SetActive(true);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
