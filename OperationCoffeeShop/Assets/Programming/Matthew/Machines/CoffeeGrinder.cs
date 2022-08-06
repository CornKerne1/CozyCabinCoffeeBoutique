using System;
using UnityEngine;


public class CoffeeGrinder : Machine
{

    private new void Start()
    {
        base.Start();
    }

    

    protected override void ChooseIngredient(GameObject other)
    {
        var pI = other.GetComponent<PhysicalIngredient>();
        switch (pI.thisIngredient)
        {
            case Ingredients.UngroundCoffee:
                currentCapacity += 1;
                machineData.outputIngredient.Add(iD.glCoffee);
                pI.playerInteraction.DropCurrentObj();
                pI.dispenser.ReleasePoolObject(pI);
                IfTutorial();
                break;
            case Ingredients.Milk:
            case Ingredients.SteamedMilk:
            case Ingredients.FoamedMilk:
            case Ingredients.Sugar:
            case Ingredients.WhippedCream:
            case Ingredients.Espresso:
            case Ingredients.GroundCoffee:
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

}