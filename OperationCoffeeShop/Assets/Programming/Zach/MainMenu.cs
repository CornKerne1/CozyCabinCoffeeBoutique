using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using System.Threading.Tasks;


public class MainMenu : MonoBehaviour
{
    [SerializeField]private string scene;
    [SerializeField]private Canvas introLetterCanvas;

    [SerializeField]private GameObject optionsScreen;

    [SerializeField]private GameObject creditsScreen;
    private GameMode _gameMode;
    [SerializeField]private GameObject wwiseBank;


    [SerializeField] private PlayableDirector director;

    private Animator _animator;

    [FormerlySerializedAs("inroLetterAnimator")]
    public Animator introLetterAnimator;

    private static readonly int Start1 = Animator.StringToHash("Start");
    private bool _loading,_gameStarted;

    //Bellow is all of the functions for managing what buttons do in the main menu.
    public void StartGame()
    {
        _gameStarted=true;
        AkSoundEngine.PostEvent("Play_MenuClick", _gameMode.gameObject);
        introLetterCanvas.enabled = false;
        _animator.SetTrigger(Start1);
        director.Play();
    }

    public async void LaunchGame()
    {
        if (_loading) return;
        AkSoundEngine.PostEvent("Stop_TitleTheme", this.gameObject);
        await Task.Delay(200);
        _loading = true;
        _gameMode.playerData.inUI = false;
         Destroy(wwiseBank);
        if(_gameMode.SaveSystem.SaveGameData.completedTutorial)
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 2);
        else
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.UnloadSceneAsync(0);
        SceneManager.UnloadSceneAsync(1);
    }

    public void OpenOptions()
    {
        if (_gameStarted) return;
        AkSoundEngine.PostEvent("Play_MenuClick", _gameMode.gameObject);
        Instantiate(optionsScreen, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void OpenCredits()
    {
        if (_gameStarted) return;
        AkSoundEngine.PostEvent("Play_MenuClick", _gameMode.gameObject);
        Instantiate(creditsScreen, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void CloseCredits()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", _gameMode.gameObject);
        creditsScreen.SetActive(false);
    }

    public void CloseOptions()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", _gameMode.gameObject);
        optionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        if (_gameStarted) return;
        AkSoundEngine.PostEvent("Play_MenuClick", _gameMode.gameObject);
        Application.Quit();
        Debug.Log("Quitting");
    }

    public void DeleteSaveFile()
    {
        AkSoundEngine.PostEvent("Play_MenuClick", _gameMode.gameObject);
        var persistentDataPath = Application.persistentDataPath + $"SaveGame{0}.json";
        if (File.Exists(persistentDataPath))
        {
            File.Delete(persistentDataPath);
            _gameMode.Load(0);
        }
    }
    private void InitializeHudAndCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        AkSoundEngine.PostEvent("Play_TitleTheme", this.gameObject);
        _animator = GetComponent<Animator>();
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _gameMode.playerData.inUI = true;
        InitializeHudAndCursor();
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
