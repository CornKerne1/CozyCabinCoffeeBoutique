using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnRegularCustomer : MonoBehaviour
{
    public GameModeData gMD;

    [FormerlySerializedAs("RCA")] public RegularCustomerAtlas regularCustomerAtlas;

    private Dictionary<int, List<GameObject>> _Time_Customer;

    private int _currentDay = -1;
    private int _currentHour = -1;


    private void Start()
    {
        DayNightCycle.HourChanged += UpdateDic;
        gMD = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().gameModeData;
    }

    private IEnumerator Spawn(GameObject customer)
    {
        yield return new WaitForSeconds(Random.Range(1, 10));
        var transform1 = transform;
        Instantiate(customer, transform1.position, transform1.rotation);
        customer.GetComponent<CustomerInteractable>().regularCustomerAtlas = regularCustomerAtlas;
    }

    private void UpdateDic(object sender, EventArgs e)
    {
        //Debug.Log("hour: " + gMD.currentTime.Hour);
        if (_currentDay != gMD.currentTime.Day)
        {
            _Time_Customer = new Dictionary<int, List<GameObject>>();
            _currentDay = gMD.currentTime.Day;
            regularCustomerAtlas.updateDictionary();
            //Debug.Log("Day: " + gMD.currentTime.Day);
            if (regularCustomerAtlas.dic.ContainsKey(gMD.currentTime.Day))
            {
                var customers = regularCustomerAtlas.dic[gMD.currentTime.Day];
                foreach (var customer in customers)
                {
                    var rc = customer.customer.GetComponent<RegularCustomer>();
                    if (rc.randomTimeOfDay)
                    {
                        var hour = Random.Range(gMD.wakeUpHour, gMD.closingHour);
                        if (!_Time_Customer.ContainsKey(hour))
                        {
                            _Time_Customer[hour] = new List<GameObject>();
                        }

                        _Time_Customer[hour].Add(customer.customer);
                    }
                    else
                    {
                        if (!_Time_Customer.ContainsKey(rc.spawnTime))
                        {
                            _Time_Customer[rc.spawnTime] = new List<GameObject>();
                        }

                        _Time_Customer[rc.spawnTime].Add(customer.customer);
                    }
                }
            }
        }

        if (gMD.isOpen && _Time_Customer.ContainsKey(gMD.currentTime.Hour))
        {
            _currentHour = gMD.currentTime.Hour;
            //Debug.Log("spawning regular customers");
            foreach (var customer in _Time_Customer[gMD.currentTime.Hour])
                StartCoroutine(Spawn(customer));
        }
        else if (_Time_Customer.ContainsKey(gMD.currentTime.Hour))
        {
            foreach (var customer in _Time_Customer[gMD.currentTime.Hour])
            {
                _Time_Customer[gMD.currentTime.Hour+1].Add(customer);
                
            }
            _Time_Customer.Remove(gMD.currentTime.Hour);
        }
    }
}