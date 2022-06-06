using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCounter : MonoBehaviour
{

    public Image dayDisplay;

    public Sprite day1;
    public Sprite day2;
    public Sprite day3;

    GameMode gM;
    private Animator animator; 
    private void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        animator = transform.root.GetComponentInChildren<Animator>();
    }


    public void DisplayDay(int day)
    {

        switch (day)
        {
            case 1:
                dayDisplay.sprite = day1;
                break;
            case 2:
                dayDisplay.sprite = day2;
                break;

            case 3:
                dayDisplay.sprite = day3;
                break;

            default:

                break;
        }
        dayDisplay.enabled = true;
    }

    public void HideDisplay()
    {
        StartCoroutine(CO_HideDisplay());
    }

    public IEnumerator CO_HideDisplay()
    { 
        animator.SetTrigger("Hide");
    yield return new WaitForSeconds(2.0f);
        Destroy(transform.root.gameObject);
    }

}
