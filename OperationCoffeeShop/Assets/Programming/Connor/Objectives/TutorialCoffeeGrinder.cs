using System;
using UnityEngine;

public class TutorialCoffeeGrinder : Machine
{
    private Objectives1 _objectives1;

    private GameMode _gameMode;

    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _objectives1 = GameObject.Find("Objectives").GetComponent<Objectives1>();
    }

    protected override void ChooseIngredient(GameObject other)
    {
        switch (other.GetComponent<PhysicalIngredient>().thisIngredient)
        {
            case Ingredients.UngroundCoffee:
                currentCapacity += 1;
                mD.outputIngredient.Add(iD.glCoffee);
                other.GetComponent<PhysicalIngredient>().pI.DropCurrentObj();
                Destroy(other);
                _objectives1.NextObjective(gameObject);
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
}