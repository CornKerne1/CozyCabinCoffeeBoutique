using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomCustomer : MonoBehaviour
{
    public GameObject customer;

    public GameModeData gMD;

    public RandomNameSet nameSet;

    public bool spawnCustomer = false;

    [SerializeField,Header("60 is once per hour")]
    public int spawnInterval = 60; //60 is once per hour

    int maxCustomerCount = 0;
    int minutes;

    bool oneCustomerAtATime = true;

    void Start()
    {
        DayNightCycle.TimeChanged += ResetMaxCustomers;
        gMD = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().gMD;
        if (maxCustomerCount == 0)
            maxCustomerCount = ((gMD.closingHour - gMD.wakeUpHour)*60)/spawnInterval;
        minutes = 0;
    }

    void Update()
    {
        if (spawnCustomer) //for manually spawning customers. 
        {
            spawnCustomer = false;
            SpawnCustomer();
        }
        if (gMD.isOpen) 
        {
            minutesSinceOpening();
            if (gMD.currentTime.TimeOfDay.Minutes == 30) //helps ensure only 1 customer spawns at a time. 
            {
                oneCustomerAtATime = true;
            }
            if (oneCustomerAtATime && minutes % spawnInterval == 0 && maxCustomerCount-- > 0) // once per interval max of count
            {
                oneCustomerAtATime = false;
                SpawnCustomer();
            }
        }
    }

    private int pastMinute = 0;

    private int minutesSinceOpening()
    {
        int currentMinute = gMD.currentTime.TimeOfDay.Minutes;
        if (currentMinute != pastMinute)
        {
            pastMinute = currentMinute;
            minutes++;
        }
        Debug.Log("minutes since opening: " + minutes +". Customers will spawn every: " + spawnInterval + " minutes. Customers remaining to spawn: "+ maxCustomerCount);
        return minutes;
    }

    public void SpawnCustomer()
    {
        StartCoroutine(Spawn());

    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
        Instantiate(customer, this.transform.position, this.transform.rotation);

    }

    void ResetMaxCustomers(object sender, EventArgs e)
    {
        if (!gMD.isOpen && gMD.wakeUpHour == gMD.currentTime.Hour )
        {
            maxCustomerCount = ((gMD.closingHour - gMD.wakeUpHour) * 60) / spawnInterval;
            minutes = 0;
            Debug.Log("reseting maxCustomerCount to :" + maxCustomerCount);
        }
    }

}
