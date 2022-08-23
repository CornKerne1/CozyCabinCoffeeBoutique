using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class DayCounter : MonoBehaviour
{
    public GameMode gameMode;
    public Image dayDisplay;

    public Sprite tutorial;
    public Sprite day1;
    public Sprite day2;
    public Sprite day3;


    private Animator _animator;
    private static readonly int Hide = Animator.StringToHash("Hide");
    private GameObject _currentDc;

    private bool _uiOn;

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _animator = transform.root.GetComponentInChildren<Animator>();
        PlayerInput.FreeCamEvent += ToggleUI;
        if (gameMode.gameModeData.inTutorial)
        {
            DisplayDay(0);
        }
        else
        {
            DisplayDay(gameMode.gameModeData.currentTime.Day);
        }

        StartCoroutine(CO_RemoveDisplayDay());
        Bed.NewDay += NewDay;
        _currentDc = gameObject;
    }

    private IEnumerator CO_RemoveDisplayDay()
    {
        yield return new WaitForSeconds(13f);
        StartCoroutine(CO_HideDisplay());
        _currentDc = null;
    }

    private void DisplayDay(int day)
    {
        dayDisplay.sprite = day switch
        {
            0 => tutorial,
            1 => day1,
            2 => day2,
            3 => day3,
            _ => dayDisplay.sprite
        };
        dayDisplay.enabled = true;
    }

    public void HideDisplay()
    {
        StartCoroutine(CO_HideDisplay());
    }

    private IEnumerator CO_HideDisplay()
    {
        _animator.SetTrigger(Hide);
        yield return new WaitForSeconds(2.0f);
        Destroy(transform.root.gameObject);
    }

    private void NewDay(object sender, EventArgs eventArgs)
    {
        if (_currentDc) return;
        _currentDc = gameObject;
        DisplayDay(gameMode.gameModeData.currentTime.Day);
        StartCoroutine(CO_RemoveDisplayDay());
    }

    private void ToggleUI(object sender, EventArgs e)
    {
        _uiOn = !_uiOn;
        try
        {
            _currentDc.GetComponentInChildren<Canvas>().enabled = !_uiOn;
        }
        catch
        {
            // ignored
        }
    }

    private void OnDisable()
    {
        PlayerInput.FreeCamEvent -= ToggleUI;
        Bed.NewDay -= NewDay;


    }
}