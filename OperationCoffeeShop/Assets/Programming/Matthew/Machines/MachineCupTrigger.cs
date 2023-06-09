using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MachineCupTrigger : MonoBehaviour
{
    private bool _takeCup;
    private IngredientContainer _cup;
    public GameMode gameMode;

    private void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    protected virtual async void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<IngredientContainer>(out var iC)) return;
        if (_takeCup)
        {
            await Task.Delay(1000);
            _takeCup = false;
            return;
        }
        _takeCup = true;
        _cup = iC;
        if(iC.playerInteraction)
            iC.playerInteraction.DropCurrentObj();
        else
            gameMode.player.GetComponent<PlayerInteraction>().DropCurrentObj();
        float elapsedTime = 0f;
        iC.transform.position = transform.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<IngredientContainer>(out var iC)) return;
        if(iC==_cup)
            _takeCup = true;
    }
}
