using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class EspressoMachine : Machine
{
    private ObjectPool<LiquidIngredients> _pool;

    private int _current;

    private new void Start()
    {
        base.Start();
        _pool = new ObjectPool<LiquidIngredients>(() =>
                Instantiate(machineData.outputIngredient[_current].GetComponentInChildren<LiquidIngredients>(),
                    outputTransform.position,
                    outputTransform.rotation), liquidIngredients =>
            {
                liquidIngredients.gameObject.SetActive(true);
                var transform1 = liquidIngredients.transform;
                transform1.position = outputTransform.position;
                transform1.rotation = outputTransform.rotation;
            },
            liquidIngredient => { liquidIngredient.gameObject.SetActive(false); }, Destroy, true, 100, 100);
    }

    protected override async Task ActivateMachine(float time)
    {
        isRunning = true;
        base.PostSoundEvent("Play_GrindingEspresso");
        int newTime = (int)(time / 1000f);
        await Task.Delay(newTime);
        OutputIngredients();
        transform.position = base.origin;
    }

    protected override void ChooseIngredient(GameObject other)
    {
        var physicalIngredient = other.GetComponent<PhysicalIngredient>();

        switch (physicalIngredient.thisIngredient)
        {
            case Ingredients.EspressoBeans:

                currentCapacity = currentCapacity + 1;
                machineData.outputIngredient.Add(iD.espresso);
                other.GetComponent<PhysicalIngredient>().playerInteraction.DropCurrentObj();
                physicalIngredient.dispenser.ReleasePoolObject(physicalIngredient);
                break;
            case Ingredients.Milk:
            case Ingredients.SteamedMilk:
            case Ingredients.FoamedMilk:
            case Ingredients.Sugar:
            case Ingredients.WhippedCream:
            case Ingredients.Espresso:
            case Ingredients.UngroundCoffee:
            case Ingredients.GroundCoffee:
            case Ingredients.Salt:
            case Ingredients.BrewedCoffee:
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
        if (currentCapacity != 0)
        {
            _current = machineData.outputIngredient.Count - 1;
            for (int k = 0; k < 20; k++)
            {
                _pool.Get();
                yield return new WaitForSeconds(.08f);
            }

            currentCapacity--;
            machineData.outputIngredient.RemoveAt(_current);
        }

        yield return new WaitForSeconds(.08f);
        base.isRunning = false;
    }
}