using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomCustomer : MonoBehaviour
{
    public GameObject customer;

    public GameModeData gMD;

    public RandomNameSet nameSet;

    public bool spawnCustomer = false;

    public int spawnInterval = 60; //60 is once per hour

    int maxCustomerCount = 0;
    int minutes;

    bool oneCustomerAtATime = true;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(customer, this.transform.position, this.transform.rotation);
        gMD = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().gMD;
        if(maxCustomerCount == 0)
            maxCustomerCount = gMD.closingHour -gMD.wakeUpHour;
        minutes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCustomer) //for manually spawning customers. 
        {
            spawnCustomer = false;
            SpawnCustomer();
        }
        if(gMD.isOpen) // if store is open
        {
            minutesSinceOpening();
            if (gMD.currentTime.TimeOfDay.Minutes == 30) //helps ensure only 1 customer spawns at a time. 
            {
                oneCustomerAtATime = true;
            }
            if (oneCustomerAtATime && minutes % spawnInterval == 0 && maxCustomerCount-- >0) // once per interval max of count
            {
                Debug.Log("spawning customer");
                oneCustomerAtATime = false;
                SpawnCustomer();
            }
        }
    }

    private int pastMinute = 0;

    private int minutesSinceOpening()
    {
        int currentMinute = gMD.currentTime.TimeOfDay.Minutes;
        if(currentMinute != pastMinute)
        {
            pastMinute = currentMinute;
            minutes++;
        }
        //Debug.Log("minutes since opening: " + minutes +". Customers will spawn every: " + spawnInterval + " minutes");
        return minutes;
    }

    public void SpawnCustomer()
    {
        Instantiate(customer, this.transform.position, this.transform.rotation);
    }
}
