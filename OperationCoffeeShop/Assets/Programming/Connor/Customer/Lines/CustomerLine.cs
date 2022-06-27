using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomerLine : MonoBehaviour
{
    public Vector3 lineStartPosition;

    private readonly Queue<CustomerAI> _queue = new Queue<CustomerAI>();

    public static event EventHandler DepositMoney;

    public Customer nextCustomer;

    private void Start()
    {
        lineStartPosition = transform.position;
    }

    public void GetInLine(GameObject customer)
    {

        var customerAI = customer.GetComponent<CustomerAI>();
        if (customerAI.customerLines.Contains(this)) return;
        customerAI.customerLines.Add(this);
        _queue.Enqueue(customerAI);
        customerAI.setDestination(GetNextSpotInLine(_queue.Count));
        customerAI.setStay(true);

    }

    public void MoveLine()
    {
       
        if (_queue.Count <= 0) return;
        var currentCustomerAI = _queue.Dequeue();
        var customerAnimations = currentCustomerAI.customerData.customerAnimations;
        currentCustomerAI.stay = false;
        if (!currentCustomerAI.hasOrdered)
        {
            currentCustomerAI.hasOrdered = true;
        }
        else if (!currentCustomerAI.hasOrder)
        {
            currentCustomerAI.hasOrder = true;
        }
        
        var i = 0;
        foreach (var customerAI in _queue)
        {
            StartCoroutine(CO_RandomMoveLine(customerAI, ++i));
            customerAnimations.RandomizeSpeed();
        }
    }

    private IEnumerator CO_RandomMoveLine(CustomerAI customerAI, int i)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(.5f,1.5f));
        customerAI.setDestination(GetNextSpotInLine(i));
    }
    
    private Vector3 GetNextSpotInLine(int position)
    {
        float i = position;
        var nextSpot = this.transform.forward * position;
        return lineStartPosition + nextSpot;

    }
    public void DeliverDrink(GameObject drink)
    {
        var drinkData = (drink).GetComponent<IngredientContainer>().dD;
        if (_queue.Count <= 0 || _queue.Peek().hasOrdered != true) return;
        var ai = _queue.Peek();
        var ci = ai.gameObject.GetComponent<CustomerInteractable>();
        ci.RemoveOrderTicket();
        ci.RemoveOrderBubble();
        ai.hasOrder = true;
        ci.DeliverDrink();
        ai.customerData.receivedDrinkData = drinkData;
        drink.SetActive(false);
        DepositMoney?.Invoke(ai.customerData.orderedDrinkData.price, EventArgs.Empty);
        ai.customerData.customer.OnReceivedDrink();
    }
    public void LeaveWithoutPaying(DrinkData drinkData)
    {
        if (_queue.Count <= 0 || _queue.Peek().hasOrdered != true) return;
        var ai = _queue.Peek();
        ai.hasOrder = true;
        MoveLine();
        ai.customerData.receivedDrinkData = drinkData;
        ai.customerData.customer.OnReceivedDrink();
    }
    public void LeaveWithoutGettingDrink(DrinkData drinkData)
    {
        if (_queue.Count <= 0 || _queue.Peek().hasOrdered != true) return;
        var ai = _queue.Peek();
        ai.hasOrder = true;
        MoveLine();
        ai.customerData.receivedDrinkData = drinkData;
        DepositMoney?.Invoke(ai.customerData.favoriteDrinkData.price, EventArgs.Empty);
        ai.customerData.customer.OnReceivedDrink();
    }
}
