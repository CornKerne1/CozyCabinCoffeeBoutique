using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Objectives : MonoBehaviour
{

     Canvas canvas;
    public List<TextMeshProUGUI> objectives;
    //public List<Image> ObjectiveImages;

    TextMeshProUGUI currentText;
    //RectTransform currentRT;

    GameMode gM;

    public int ObjectiveCount = 0;

    float dist;

    private bool closingObjective = false;
    private bool openingObjective = false;


    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        canvas = gameObject.GetComponent<Canvas>();
        foreach (TextMeshProUGUI text in objectives)
        {
            text.alpha = 0;
        }
        if (gM.gMD.currentTime.Day == 1)
        {
            currentText = objectives[0];
            currentText.alpha = 255;
            ObjectiveCount = 0;
        }


    }

    void Update()
    {
        if (gM.gMD.currentTime.Hour >= gM.gMD.wakeUpHour && gM.gMD.currentTime.Minute >= 1 && !openingObjective)
        {
            changeObjective(++ObjectiveCount);
            openingObjective = true;
        }
        if (gM.gMD.currentTime.Hour>= gM.gMD.closingHour && !closingObjective)
        {
            changeObjective(++ObjectiveCount);
            closingObjective = true;
        }
    }


    public void changeObjective(int i)
    {
        currentText.alpha = 0;
        currentText = objectives[i];
        currentText.alpha = 255;
    }
}
