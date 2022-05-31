using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspressoMachine : Machine
{
    public override IEnumerator ActivateMachine(float time)
    {
        isRunning = true;
        base.PostSoundEvent("Play_GrindingEspresso");
        yield return new WaitForSeconds(time);
        OutputIngredients();
        transform.position = base.origin;
    }
    public override void ChooseIngredient(GameObject other)
    {
        switch (other.GetComponent<PhysicalIngredient>().thisIngredient)
        {
            case Ingredients.EspressoBeans:

                currentCapacity = currentCapacity + 1;
                mD.outputIngredient.Add(iD.espresso);
                other.GetComponent<PhysicalIngredient>().pI.DropCurrentObj();
                Destroy(other);
                break;
        }
    }
    public override void OutputIngredients()
    {
        StartCoroutine(Liquify());
    }
    private IEnumerator Liquify()
    {
        if (currentCapacity != 0)
        {
            int current = mD.outputIngredient.Count - 1;
            for (int k = 0; k < 20; k++)
            {
                Instantiate(mD.outputIngredient[current], outputTransform.position, outputTransform.rotation);
                yield return new WaitForSeconds(.08f);
            }
            currentCapacity--;
            mD.outputIngredient.RemoveAt(current);
        }
        yield return new WaitForSeconds(.08f);
        base.isRunning = false;
    }
}
