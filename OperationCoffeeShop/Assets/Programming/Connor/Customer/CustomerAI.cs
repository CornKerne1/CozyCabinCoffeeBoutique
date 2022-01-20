using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAI : MonoBehaviour
{
    private Vector3 startingPosition;

    public CustomerData customerData;

    public CustomerAI(Vector3 startingPosition)
    {
        this.startingPosition = startingPosition;
    }
    public CustomerAI(Vector3 startingPosition, CustomerData CD)
    {
        this.startingPosition = startingPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
       // this.customerData = gameO
    }

}
