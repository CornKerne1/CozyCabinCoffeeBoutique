using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialCoffeeBrewer : Machine
{
    public bool hasPitcher;

    public GameObject objectiveOutputObject;
    private Objectives1 _objectives1;

    private void Start()
    {
        _objectives1 = GameObject.Find("Objectives").GetComponent<Objectives1>();
    }

    protected override IEnumerator ActivateMachine(float time)
    {
        isRunning = true;
        yield return new WaitForSeconds(time);
        OutputIngredients();
        transform.position = base.origin;
    }

    protected override void ChooseIngredient(GameObject other)
    {
        Debug.Log("we got this far");
        switch (other.GetComponent<TutorialPhysicalIngredient>().thisIngredient)
        {
            case Ingredients.GroundCoffee:

                currentCapacity = currentCapacity + 1;
                mD.outputIngredient.Add(iD.brewedCoffee);
                other.GetComponent<TutorialPhysicalIngredient>().pI.DropCurrentObj();
                Destroy(other);
                _objectives1.NextObjective(objectiveOutputObject);

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

    protected override void OutputIngredients()
    {
        StartCoroutine(Liquify());
    }

    private IEnumerator Liquify()
    {
        for (int i = 0; i < currentCapacity;)
            if (currentCapacity != 0)
            {
                for (int k = 0; k < 100 * (i + 1); k++)
                {
                    Instantiate(mD.outputIngredient[i], outputTransform.position, outputTransform.rotation);
                    yield return new WaitForSeconds(.04f);
                }

                currentCapacity--;
                mD.outputIngredient.RemoveAt(i);
            }

        yield return new WaitForSeconds(.04f);
        base.isRunning = false;
    }
}