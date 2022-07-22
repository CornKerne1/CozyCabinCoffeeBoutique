using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBed : Interactable
{
    private GameObject _currentDc;

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        //if (gameMode.objectives.objectives.Count - 1 != gameMode.objectives.currentObjective) return;
        AkSoundEngine.PostEvent("STOP_DREAMSCAPE", gameObject);
        AkSoundEngine.PostEvent("PLAY_WAKING", gameObject);
        StartCoroutine(CO_LoadNextScene());
    }

    private IEnumerator CO_LoadNextScene()
    {
        yield return new WaitForSeconds(.1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}