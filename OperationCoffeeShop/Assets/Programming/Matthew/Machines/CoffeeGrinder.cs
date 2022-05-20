using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeGrinder : Machine
{
    public override void ChooseIngredient(GameObject other)
    {
        switch (other.GetComponent<PhysicalIngredient>().thisIngredient)
        {
            case Ingredients.UngroundCoffee:
                currentCapacity = currentCapacity + 1;
                mD.outputIngredient.Add(iD.glCoffee);
                Debug.Log("Test333");
                other.GetComponent<PhysicalIngredient>().pI.DropCurrentObj();
                Debug.Log("Test22222");
                Destroy(other);
                Debug.Log("Test1111");
                break;
        }
    }
}
