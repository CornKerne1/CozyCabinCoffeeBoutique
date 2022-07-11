using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeBrewer : Machine
{
    public bool hasPitcher;

    protected override IEnumerator ActivateMachine(float time)
    {
        isRunning = true;
        yield return new WaitForSeconds(time);
        OutputIngredients();
        transform.position = base.origin;
    }

    protected override void ChooseIngredient(GameObject other)
    {
        switch (other.GetComponent<PhysicalIngredient>().thisIngredient)
        {
            case Ingredients.GroundCoffee:
                IfTutorial();
                currentCapacity = currentCapacity + 1;
                mD.outputIngredient.Add(iD.brewedCoffee);
                other.GetComponent<PhysicalIngredient>().pI.DropCurrentObj();
                Destroy(other);
                
                break;
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
