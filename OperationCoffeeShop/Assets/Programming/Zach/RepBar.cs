using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RepBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxRep(int Maxrep)
    {
        slider.maxValue = Maxrep;
    }
    public void SetRep(int reputation)
    {
        slider.value = reputation;
    }
}
