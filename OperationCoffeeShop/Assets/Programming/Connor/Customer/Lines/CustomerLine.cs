using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class CustomerLine : MonoBehaviour
{
    public Vector3 lineStartPosition;

    Queue<CustomerAI> queue = new Queue<CustomerAI>();

    public static event EventHandler DrinkBeGone;

    public static event EventHandler DepositMoney;//

    //used for testing
    public bool next = false;

    public Customer NextCustomer;

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

        CustomerAI customerAI = customer.GetComponent<CustomerAI>();
        //Debug.Log("get in line");
        if (!customerAI.customerLines.Contains(this))
        {
            customerAI.customerLines.Add(this);
            queue.Enqueue(customerAI);
            //Debug.Log("que count is: " + queue.Count);
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
            CustomerAnimations cA = ai.gameObject.GetComponent<CustomerAnimations>();
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
                StartCoroutine(RandomMoveLine(AI, ++i));
                cA.RandomizeSpeed();
            }
            i = 0;
        }
    }

    IEnumerator RandomMoveLine(CustomerAI AI, int i)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(.5f,1.5f));
        AI.setDestination(getNextSpotInLine(i));
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
    public void deliverDrink(GameObject drink)
    {
        DrinkData drinkdata = (drink).GetComponent<IngredientContainer>().dD;
        if (queue.Count > 0 && queue.Peek().hasOrdered == true)
        {
            CustomerAI ai = queue.Peek();
            CustomerInteractable ci = ai.gameObject.GetComponent<CustomerInteractable>();
            ci.RemoveOrderTicket();
            ci.RemoveOrderBubble();
            ai.hasOrder = true;
            ci.DeliverDrink();
            ai.CD.recievedDrink = drinkdata;
            drink.SetActive(false);
            DepositMoney?.Invoke(ai.CD.favoriteDrink.price, EventArgs.Empty);
            ai.CD.customer.RecieveDrink();

        }
    }
    public void LeaveWithoutPaying(DrinkData drinkdata)
    {
        if (queue.Count > 0 && queue.Peek().hasOrdered == true)
        {
            CustomerAI ai = queue.Peek();
            ai.hasOrder = true;
            moveLine();
            ai.CD.recievedDrink = drinkdata;
            ai.CD.customer.RecieveDrink();

        }
    }
    public void LeaveWithoutGettingDrink(DrinkData drinkdata)
    {
        if (queue.Count > 0 && queue.Peek().hasOrdered == true)
        {
            CustomerAI ai = queue.Peek();
            ai.hasOrder = true;
            moveLine();
            ai.CD.recievedDrink = drinkdata;
            DepositMoney?.Invoke(ai.CD.favoriteDrink.price, EventArgs.Empty);
            ai.CD.customer.RecieveDrink();

        }
    }
}
