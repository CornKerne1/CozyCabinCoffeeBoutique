using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoffeeBrewer : Machine
{
    public bool hasPitcher;
    private ObjectPool<GameObject> _pool;


    private int _iterations;

    private new void Start()
    {
        base.Start();
        _pool = new ObjectPool<GameObject>(() =>
            {
                return Instantiate(machineData.outputIngredient[_iterations], outputTransform.position,
                    outputTransform.rotation);
                ;
            }, gameObject =>
            {
                gameObject.SetActive(true);
                gameObject.transform.position = outputTransform.position;
                gameObject.transform.rotation = outputTransform.rotation;
            },
            gameObject => { gameObject.SetActive(false); }, gameObject => { Destroy(gameObject); }, true, 100, 100);
    }

    protected override IEnumerator ActivateMachine(float time)
    {
        AkSoundEngine.PostEvent("PLAY_Brewer", gameObject);
        isRunning = true;
        yield return new WaitForSeconds(time);
        AkSoundEngine.PostEvent("STOP_Brewer", gameObject);
        OutputIngredients();
        transform.position = base.origin;
    }
//
    protected override void ChooseIngredient(GameObject other)
    {
        var physicalIngredient = other.GetComponent<PhysicalIngredient>();
        switch (physicalIngredient.thisIngredient)
        {
            case Ingredients.GroundCoffee:
                IfTutorial();
                currentCapacity++;
                machineData.outputIngredient.Add(iD.brewedCoffee);
                other.GetComponent<PhysicalIngredient>().playerInteraction.DropCurrentObj();
                physicalIngredient.machine.ReleasePoolObject(other.transform.root.gameObject);

                break;
            case Ingredients.Milk:
            case Ingredients.SteamedMilk:
            case Ingredients.FoamedMilk:
            case Ingredients.Sugar:
            case Ingredients.WhippedCream:
            case Ingredients.Espresso:
            case Ingredients.UngroundCoffee:
            case Ingredients.Salt:
            case Ingredients.BrewedCoffee:
            case Ingredients.EspressoBeans:
            case Ingredients.CoffeeFilter:
            case Ingredients.TeaBag:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void IfTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }


    protected override void OutputIngredients()
    {
        StartCoroutine(CO_Liquefy());
    }

    private IEnumerator CO_Liquefy()
    {
        AkSoundEngine.PostEvent("PLAY_LOOPPOUR", gameObject);
        for (_iterations = 0; _iterations < currentCapacity;)
            if (currentCapacity != 0)
            {
                for (var k = 0; k < 100 * (_iterations + 1); k++)
                {
                    _pool.Get();
                    yield return new WaitForSeconds(.04f);
                }

                currentCapacity--;
                machineData.outputIngredient.RemoveAt(_iterations);
            }

        yield return new WaitForSeconds(.04f);
        base.isRunning = false;
        AkSoundEngine.PostEvent("STOP_LOOPPOUR", gameObject);
    }
}