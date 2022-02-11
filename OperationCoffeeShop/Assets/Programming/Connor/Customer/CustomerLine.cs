using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CustomerLine : MonoBehaviour
{
    public Vector3 lineStartPosition;

    List<CustomerAI> line = new List<CustomerAI>();



    // Start is called before the first frame update
    void Start()
    {
        lineStartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getInLine(GameObject customer)
    {
       CustomerAI customerAI = customer.GetComponent<CustomerAI>();
       customerAI.setDestination(getNextSpotInLine());
        customerAI.setStay(true);//sets stay
    }

    public void moveLine()
    {
        //moves each customer up 1 space in line 
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 getNextSpotInLine()
    {
        float i =line.Count+1;
        Vector3 nextSpot = new Vector3(0f,0f,i);
        return lineStartPosition + nextSpot;
    }

}
