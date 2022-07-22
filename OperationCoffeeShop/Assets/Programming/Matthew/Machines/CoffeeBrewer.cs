using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeBrewer : Machine
{
    public bool hasPitcher;

    private new void Start()
    {
        base.Start();
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

    protected override void ChooseIngredient(GameObject other)
    {
        var pI = other.GetComponent<PhysicalIngredient>();
        switch (pI.thisIngredient)
        {
            case Ingredients.GroundCoffee:
                IfTutorial();
                currentCapacity++;
                mD.outputIngredient.Add(iD.brewedCoffee);
                other.GetComponent<PhysicalIngredient>().playerInteraction.DropCurrentObj();
                pI.dispenser.ReleasePoolObject(other.transform.root.gameObject);

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
        for (var i = 0; i < currentCapacity;)
            if (currentCapacity != 0)
            {
                for (var k = 0; k < 100 * (i + 1); k++)
                {
                    Instantiate(mD.outputIngredient[i], outputTransform.position, outputTransform.rotation);
                    yield return new WaitForSeconds(.04f);
                }

                currentCapacity--;
                mD.outputIngredient.RemoveAt(i);
            }

        yield return new WaitForSeconds(.04f);
        base.isRunning = false;
        AkSoundEngine.PostEvent("STOP_LOOPPOUR", gameObject);
    }
}