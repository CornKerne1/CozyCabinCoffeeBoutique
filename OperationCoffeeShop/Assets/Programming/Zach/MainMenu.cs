using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Serialization;


public class MainMenu : MonoBehaviour
{
    public string scene;

    public Canvas introLetterCanvas;

    public GameObject optionsScreen;

    public GameObject creditsScreen;
    private GameMode _gameMode;


    [SerializeField] private PlayableDirector director;

    private Animator _animator;

    [FormerlySerializedAs("inroLetterAnimator")]
    public Animator introLetterAnimator;

    private static readonly int Start1 = Animator.StringToHash("Start");

    //Bellow is all of the functions for managing what buttons do in the main menu.
    public void StartGame()
    {
        introLetterCanvas.enabled = false;
        _animator.SetTrigger(Start1);
        director.Play();
        Cursor.visible = false;
    }

    public void LaunchGame()
    {
        AkSoundEngine.PostEvent("Stop_TitleTheme", this.gameObject);
        if(_gameMode.SaveSystem.SaveGameData.completedTutorial)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 2);
        else
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenOptions()
    {
        Instantiate(optionsScreen, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void OpenCredits()
    {
        Instantiate(creditsScreen, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void CloseCredits()
    {
        creditsScreen.SetActive(false);
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
        _animator = GetComponent<Animator>();
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _gameMode.gameModeData.isOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        director.stopped += ReadNote;
    }

    private void ReadNote(PlayableDirector aDirector)
    {
        Cursor.visible = true;
        introLetterCanvas.enabled = true;
        introLetterAnimator.enabled = true;
    }
}