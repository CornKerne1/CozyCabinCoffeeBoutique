using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipJar : Interactable
{
    // Start is called before the first frame update
    private PlayerInteraction pI;
    [SerializeField] private GameObject halfJar;
    [SerializeField] private GameObject fullJar;

    public override void OnInteract(PlayerInteraction pI)
    {
        this.pI = pI;
        pI.Carry(gameObject);
    }

    public override void OnAltInteract(PlayerInteraction pI)
    {
        
    }
    

    private void Update()
    {
        if (gM.gMD.reputation > 5)
        {
            fullJar.SetActive(true);
            halfJar.SetActive(false);
        }
        else
        {
            halfJar.SetActive(true);
            fullJar.SetActive(false);
        }
    }
}
