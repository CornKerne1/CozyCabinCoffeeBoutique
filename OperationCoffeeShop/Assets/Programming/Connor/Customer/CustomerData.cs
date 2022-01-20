using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "CustomerData/Generic")]
public class CustomerData : ScriptableObject
{
    public string Name;
    public Customer CustomerInfo;
    public CustomerAI CAI;
    public CustomerData( Customer customerInfo)
    {
        this.CustomerInfo = customerInfo;
       
    }

    public void setAI(CustomerAI CAI)
    {
        if ( CAI != null)
        {
            this.CAI = CAI;
        }
    }
}
