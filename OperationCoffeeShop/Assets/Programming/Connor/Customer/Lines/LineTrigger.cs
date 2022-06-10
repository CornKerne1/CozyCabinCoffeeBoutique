using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTrigger : MonoBehaviour
{
    private CustomerLine _customerLine;

    [SerializeField] private string customerTag = "Customer";

    // Start is called before the first frame update
    private void Start()
    {
        _customerLine = GetComponentInParent<CustomerLine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Customer")) {
            //Debug.Log("Yes I love Customers in line!!");
            _customerLine.GetInLine(other.gameObject);
        }
    }
}
