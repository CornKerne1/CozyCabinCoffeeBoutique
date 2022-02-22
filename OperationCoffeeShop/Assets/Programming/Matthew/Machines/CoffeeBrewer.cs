using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeBrewer : Machine
{
    public override void ChooseIngredient(GameObject other)
    {
        switch (other.GetComponent<PhysicalIngredient>().thisIngredient)
        {
            case Ingredients.GroundCoffee:

                currentCapacity = currentCapacity + 1;
                mD.outputIngredient.Add(iD.glCoffee);
                other.GetComponent<PhysicalIngredient>().pI.DropCurrentObj();
                Destroy(other);
                break;
        }
    }
}
