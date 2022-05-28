using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SpawnRegularCustomer : MonoBehaviour
{

    public GameModeData gMD;

    public RegularCustomerAtlas RCA;

    public Dictionary<int, List<GameObject>> Time_Customer;

    private int currentDay = -1;
    private int currentHour = -1;

    // Start is called before the first frame update
    void Start()
    {
        DayNightCycle.TimeChanged += UpdateDic;
        gMD = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().gMD;
    }

    private IEnumerator Spawn(GameObject customer)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 10));
        Instantiate(customer, this.transform.position, this.transform.rotation);

    }
    private void UpdateDic(object sender, EventArgs e)
    {
        Debug.Log("hour: " + gMD.currentTime.Hour);
        if (currentDay != gMD.currentTime.Day)
        {
            Time_Customer = new Dictionary<int, List<GameObject>>();
            currentDay = gMD.currentTime.Day;
            RCA.updateDictionary();
            Debug.Log("Day: " + gMD.currentTime.Day);
            if (RCA.dic.ContainsKey(gMD.currentTime.Day))
            {
                List<GameObject> customers = RCA.dic[gMD.currentTime.Day];
                foreach (GameObject customer in customers)
                {
                    RegularCustomer rc = customer.GetComponent<RegularCustomer>();
                    if (rc.randomTimeOfDay)
                    {
                        int hour = UnityEngine.Random.Range(gMD.wakeUpHour, gMD.closingHour);
                        if (!Time_Customer.ContainsKey(hour))
                        {
                            Time_Customer[hour] = new List<GameObject>();
                        }
                        Time_Customer[hour].Add(customer);
                    }
                    else
                    {
                        if (Time_Customer[rc.spawnTime] == null)
                        {
                            Time_Customer[rc.spawnTime] = new List<GameObject>();
                        }
                        Time_Customer[rc.spawnTime].Add(customer);
                    }
                }
            }
        }
        if (currentHour != gMD.currentTime.Hour && Time_Customer.ContainsKey(gMD.currentTime.Hour))
        {
            currentHour = gMD.currentTime.Hour;
            Debug.Log("spawning regular customers");
            foreach (GameObject customer in Time_Customer[gMD.currentTime.Hour])
            StartCoroutine(Spawn(customer));
        }
    }
}
