using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGrinder : Machine
{
    public override void ChooseIngredient(GameObject other)
    {
        var obj = other;
        switch (other.GetComponent<PhysicalIngredient>().thisIngredient)
        {
            case Ingredients.UngroundCoffee:

                currentCapacity = currentCapacity + 1;
                mD.outputIngredient.Add(iD.glCoffee);
                other.GetComponent<PhysicalIngredient>().pI.DropCurrentObj();
                Destroy(other);
                new WaitForSeconds(1f);
                return;
        }
    }
}
