using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public int currentCapacity;
    public MachineData mD;
    public bool isRunning;


    public Transform outputTransform;

    // Start is called before the first frame update
    public void Interact(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Interactable") && other.tag == "PickUp" && currentCapacity < mD.maxCapacity && !isRunning)
        {
            if(other.GetComponent<PhysicalIngredient>().iD.thisIngredient == mD.acceptedIngredient)
            {
                Debug.Log("wth");
                currentCapacity = currentCapacity + 1;
                Destroy(other);
            }
        }
    }

    public void StartMachine(float time)
    {
        if(!isRunning)
        {
            StartCoroutine(ActivateMachine(time));
        }
    }


    private IEnumerator ActivateMachine(float time)
    {
        isRunning = true;
        yield return new WaitForSeconds(time);
        OutputIngredients();
        isRunning = false;
    }

    private void OutputIngredients()
    {
        for (int i = 0; i <= currentCapacity; i++)
            if (currentCapacity == 0)
            {

            }
            else
            {
                currentCapacity = currentCapacity - 1;
                Debug.Log(currentCapacity);
                Instantiate(mD.outputIngredient, outputTransform.position, outputTransform.rotation);
            }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentCapacity);
    }
}
