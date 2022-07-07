using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConsumption : MonoBehaviour
{
    

    
    //tutorial stuffs
    private Objectives1 _objectives1;
    private bool _inTutorial;
    private bool _completedObjective;
    
    
    private void Start()
    {
      IfTutorial();
    }
    private void IfTutorial()
    {
        try
        {
            _objectives1 = GameObject.Find("Objectives").GetComponent<Objectives1>();
            _inTutorial = true;
        }
        catch
        {
            // ignored
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<LiquidIngredients>(out var liquid))
        {
            liquid.gameObject.SetActive(false); 
            Debug.Log("Drinking the coffee");
            if (_inTutorial && !_completedObjective)
            {
                _completedObjective = true;
                _objectives1.NextObjective(gameObject);

            }
        }
    }
}