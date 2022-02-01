using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRaandomCustomer : MonoBehaviour
{
    public GameObject customer;

    public RandomNameSet nameSet;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(customer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
