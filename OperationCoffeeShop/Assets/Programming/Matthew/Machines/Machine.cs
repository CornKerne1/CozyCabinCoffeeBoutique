using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public int currentCapacity;
    public MachineData mD;

    public GameObject outputIngredient;

    public Transform outputTransform;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ingredient") && currentCapacity < mD.maxCapacity)
        {
            if(other.GetComponent<PhysicalIngredient>().iD.thisIngredient == mD.acceptedIngredient)
            {
                currentCapacity += currentCapacity;
                Destroy(other);
            }
        }
    }

    public IEnumerator ActivateMachine(float time)
    {
        yield return new WaitForSeconds(time);
        OutputIngredients();
    }

    private void OutputIngredients()
    {
        for (int i = currentCapacity; i > 0; i--)
            currentCapacity = i;
            Instantiate(outputIngredient, outputTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
