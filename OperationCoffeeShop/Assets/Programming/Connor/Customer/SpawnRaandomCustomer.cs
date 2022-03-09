using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRaandomCustomer : MonoBehaviour
{
    public GameObject customer;

    public RandomNameSet nameSet;

    public bool spawnCustomer = false;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(customer, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCustomer)
        {
            spawnCustomer = false;
            SpawnCustomer();
        }
    }

    public void SpawnCustomer()
    {
        Instantiate(customer, gameObject.transform);
    }
}
