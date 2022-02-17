using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CustomerLine : MonoBehaviour
{
    public Vector3 lineStartPosition;

    Queue<CustomerAI> queue = new Queue<CustomerAI>();

    List<SpotInLine> spots = new List<SpotInLine>();

    //used for testing
    public bool next = false;

    public int maxLineSize = 5;
    /// <summary>
    /// only for testing delete when no lonker needed
    /// </summary>
    private void Update()
    {
        if (next == true)
        {
            next = false;
            moveLine();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineStartPosition = transform.position;
    }
    /// <summary>
    /// Puts the customer in the next spot in line. 
    /// </summary>
    /// <param name="customer"></param>
    public void getInLine(GameObject customer)
    {
        if (queue.Count < maxLineSize)
        {
            CustomerAI customerAI = customer.GetComponent<CustomerAI>();

            customerAI.customerLine = this;
            queue.Enqueue(customerAI);
            Debug.Log("que count is" + queue.Count);
            customerAI.setDestination(getNextSpotInLine(queue.Count));
            customerAI.setStay(true);//sets stay
        }
        //if line is full
        else
        {
            Debug.Log("line is at capacity");
        }
    }

    /// <summary>
    /// dequeues the 1st person in line and moves each customer up 1 position. 
    /// </summary>
    public void moveLine()
    {
        int i = 0;
        if (queue.Count > 0)
        {
            CustomerAI ai = queue.Dequeue();
            ai.stay = false;
            ai.customerLine = null;
            foreach (CustomerAI AI in queue)
            {
                //moves customer up 1 position & incraments position. 
                AI.setDestination(getNextSpotInLine(i++));
            }
            i = 0;
        }
    }

    /// <summary>
    /// returns the appropriate spot in line for a particular postion in line. 
    /// </summary>
    public Vector3 getNextSpotInLine(int position)
    {

        float i = position;
        Vector3 nextSpot = this.transform.forward * position;
        Debug.Log("customer " + position + " is:" + lineStartPosition + nextSpot);

        return lineStartPosition + nextSpot;

    }


    /// <summary>
    /// A better way to make lines in the future would be to link list them. 
    /// </summary>
    class SpotInLine
    {
        Vector3 spot;
        bool occupied;

        void CreateSpotInLine(Transform trans, float distance)
        {
            spot = trans.position + trans.forward * distance;

        }
    }
}
