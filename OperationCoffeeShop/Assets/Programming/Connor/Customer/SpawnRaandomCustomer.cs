using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRaandomCustomer : MonoBehaviour
{
    GameObject customer;

    public RandomNameSet nameSet;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(customer);
        Customer RC = new RandomCustomer(nameSet);
        CustomerData CD = new CustomerData(RC);
        CustomerAI CAI = new CustomerAI(this.transform.position, CD);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
