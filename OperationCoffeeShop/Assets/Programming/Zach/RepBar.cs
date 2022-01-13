using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RepBar : MonoBehaviour
{
    public int Maxrep;
    public Slider slider;

    public void SetMaxRep(int reputation)
    {
        slider.maxValue = Maxrep;
        slider.value = reputation;
    }
    public void SetRep(int reputation)
    {
        slider.value = reputation;
    }
}
