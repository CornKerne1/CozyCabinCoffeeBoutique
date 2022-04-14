using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class CustomerLine : MonoBehaviour
{
    public Vector3 lineStartPosition;

    Queue<CustomerAI> queue = new Queue<CustomerAI>();

    [SerializeField] public static event EventHandler DrinkBeGone;

    [SerializeField] public static event EventHandler DepositMoney;//
 
    //used for testing
    public bool next = false;


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
        DeliveryZone.DrinkDelivery += deliverDrink;
    }
    /// <summary>
    /// Puts the customer in the next spot in line. 
    /// </summary>
    /// <param name="customer"></param>
    public void getInLine(GameObject customer)
    {

        CustomerAI customerAI = customer.GetComponent<CustomerAI>();
        Debug.Log("get in line");
        if (!customerAI.customerLines.Contains(this))
        {
            customerAI.customerLines.Add(this);
            queue.Enqueue(customerAI);
            Debug.Log("que count is: " + queue.Count);
            customerAI.setDestination(getNextSpotInLine(queue.Count));
            customerAI.setStay(true);//sets stay
        }
        //if line is full

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
            if (!ai.hasOrdered)
            {
                ai.hasOrdered = true;
            }
            else if (!ai.hasOrder)
            {
                ai.hasOrder = true;
            }
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
        //Debug.Log("customer " + position + " is:" + lineStartPosition + nextSpot);

        return lineStartPosition + nextSpot;

    }


    /// <summary>
    /// 
    /// </summary>
    private void deliverDrink(object sender, EventArgs e)
    {
        DrinkData drink = ((GameObject)sender).GetComponent<IngredientContainer>().dD;
        //Debug.Log("recieved event");
        if (queue.Count>0 && queue.Peek().hasOrdered == true)
        {
            //Debug.Log("Deliverd drink");
            CustomerAI ai = queue.Peek();
            ai.hasOrder = true;
            moveLine();
            ai.CD.recievedDrink = (DrinkData)drink;
            ((GameObject)sender).active = false;
            DepositMoney?.Invoke(ai.CD.favoriteDrink.price, EventArgs.Empty);
            DrinkBeGone?.Invoke(ai.CD.recievedDrink, EventArgs.Empty);

            Debug.Log("Here is your money");//

        }
    }
}
