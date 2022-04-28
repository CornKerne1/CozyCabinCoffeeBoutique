using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RepBar : MonoBehaviour
{

    public Slider slider;

    private void Start()
    {
        Customer.CustomerRating += SetRepHelper;

    }
    private void SetRepHelper(object sender, EventArgs e)
    {
        int x;
        if ((float)sender > .5)
            x = 1;
        else x = -1;
        //Debug.Log((int)slider.value +" " + x);
        SetRep((int)slider.value+x); 
    }
    public void SetMaxRep(int Maxrep)
    {
        slider.maxValue = Maxrep;
    }
    public void SetRep(int reputation)
    {
        //Debug.Log("proccessing Reputation");
        slider.value = reputation;
    }
}
