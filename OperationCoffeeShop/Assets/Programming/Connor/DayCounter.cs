using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayCounter : MonoBehaviour
{
    public Image dayDisplay;

    public Sprite day1;
    public Sprite day2;
    public Sprite day3;

    private GameMode _gameMode;
    private Animator _animator;
    private static readonly int Hide = Animator.StringToHash("Hide");

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _animator = transform.root.GetComponentInChildren<Animator>();
    }


    public void DisplayDay(int day)
    {
        dayDisplay.sprite = day switch
        {
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

    public IEnumerator CO_HideDisplay()
    {
        _animator.SetTrigger(Hide);
        yield return new WaitForSeconds(2.0f);
        Destroy(transform.root.gameObject);
    }
}