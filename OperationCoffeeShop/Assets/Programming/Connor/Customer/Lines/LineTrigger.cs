using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTrigger : MonoBehaviour
{
    CustomerLine line;

    string customerTag = "Customer";

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponentInParent<CustomerLine>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Customer")) {
            Debug.Log("Yes I love Customers in line!!");
            line.getInLine(other.gameObject);
        }
    }
}
