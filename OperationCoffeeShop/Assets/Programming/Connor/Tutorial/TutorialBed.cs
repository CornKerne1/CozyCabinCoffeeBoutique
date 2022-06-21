using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DayCounter))]
public class TutorialBed : Interactable
{
    public GameObject dayCounter;
    private DayCounter _dayCounter;
    private GameObject _currentDc;


    public override void Start()
    {
        base.Start();
        _currentDc = Instantiate(dayCounter);
        _dayCounter = _currentDc.GetComponent<DayCounter>();
        _dayCounter.DisplayDay(gM.gameModeData.currentTime.Day);
        _dayCounter.DisplayDay(0);
        StartCoroutine(CO_RemoveDisplayDay());
    }

    private IEnumerator CO_RemoveDisplayDay()
    {
        yield return new WaitForSeconds(13f);
        StartCoroutine(_dayCounter.CO_HideDisplay());
        _currentDc = null;
    }

    public override void OnInteract(PlayerInteraction playerInteraction)
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    }
}