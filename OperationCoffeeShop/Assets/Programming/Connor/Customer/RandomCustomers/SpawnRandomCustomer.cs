using System;
using System.Collections;
using UnityEngine;

public class SpawnRandomCustomer : MonoBehaviour
{
    public GameObject customer;

    public GameModeData gMD;

    public RandomNameSet nameSet;

    public bool spawnCustomer;

    [SerializeField, Header("60 is once per hour")]
    public int spawnInterval = 60; //60 is once per hour

    private int _maxCustomerCount;
    private int _minutes;
    private int _pastMinute;

    private bool _oneCustomerAtATime = true;

    [SerializeField] private GameObject customerPath;

    private void Start()
    {
        DayNightCycle.TimeChanged += ResetMaxCustomers;
        gMD = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().gameModeData;
        if (_maxCustomerCount == 0)
            _maxCustomerCount = ((gMD.closingHour - gMD.wakeUpHour) * 60) / spawnInterval;
        _minutes = 0;
        Button.OpenShop += SpawnInitialCustomer;
    }

    private void Update()
    {
        if (spawnCustomer)
        {
            spawnCustomer = false;
            SpawnCustomer();
        }

        if (!gMD.isOpen) return;
        MinutesSinceOpening();
        if (gMD.currentTime.TimeOfDay.Minutes == 30)
        {
            _oneCustomerAtATime = true;
        }

        if (!_oneCustomerAtATime || _minutes % spawnInterval != 0 || _maxCustomerCount-- <= 0) return;
        _oneCustomerAtATime = false;
        SpawnCustomer();
    }


    private void MinutesSinceOpening()
    {
        var currentMinute = gMD.currentTime.TimeOfDay.Minutes;
        if (currentMinute == _pastMinute) return;
        _pastMinute = currentMinute;
        _minutes++;

        //Debug.Log("minutes since opening: " + _minutes +". Customers will spawn every: " + spawnInterval + " minutes. Customers remaining to spawn: "+ _maxCustomerCount);// uncommenting this will give the minutes since open, for bug testing only  
    }

    private void SpawnCustomer()
    {
        StartCoroutine(CO_Spawn());
    }

    private IEnumerator CO_Spawn()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
        var transform1 = transform;
        Instantiate(customer, transform1.position, transform1.rotation);

        customer.GetComponent<CustomerAI>().PathConditioning(customerPath);
    }

    private void ResetMaxCustomers(object sender, EventArgs e)
    {
        if (gMD.isOpen || gMD.wakeUpHour != gMD.currentTime.Hour) return;
        _maxCustomerCount = ((gMD.closingHour - gMD.wakeUpHour) * 60) / spawnInterval;
        _minutes = 0;
    }

    private void SpawnInitialCustomer(object sender, EventArgs eventArgs)
    {
        SpawnCustomer();
    }
}