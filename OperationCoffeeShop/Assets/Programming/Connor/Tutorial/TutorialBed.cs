using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBed : Interactable
{
    private GameObject _currentDc;

    [SerializeField] private bool loadingNextScene;
    [SerializeField] private GameObject wwiseBank;

    public override void OnInteract(PlayerInteraction interaction)
    {
        
        if (gameMode.objectives.objectives.Count - 1 != gameMode.objectives.currentObjective) return;
        if(loadingNextScene) return;
        gameMode.SaveSystem.SaveGameData.completedTutorial = true;
        gameMode.Save(0);
        loadingNextScene = true;
        AkSoundEngine.PostEvent("STOP_DREAMSCAPE", gameObject);
        AkSoundEngine.PostEvent("PLAY_WAKING", gameObject);
       
        StartCoroutine(CO_LoadNextScene());
    }

    private IEnumerator CO_LoadNextScene()
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(wwiseBank);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}