using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyLancher : MonoBehaviour
{

    GameMode gM;

    public GameObject dollarBill;
    public GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

    }

    public void LaunchMoney(int dollars, int change)
    {
        Debug.Log("dollars:" + dollars + " cents: " + change);
        List<GameObject> money = new List<GameObject>();
        while (dollars-- > 0 && dollarBill!= null)
        {
            Debug.Log("zach");
            GameObject go = Instantiate(dollarBill, transform.position,transform.rotation);
            money.Add(go);
        }
        while (change-- > 0 && coin!=null)
        {
            Debug.Log("thanstrom");
            GameObject go = Instantiate(coin);
            money.Add(go);
        }
        foreach(GameObject m in money)
        {
            Debug.Log("sorry i miss spelled your name zaaaaccchhhh");
            Vector3 torque = new Vector3();
            Rigidbody rb = m.GetComponent<Rigidbody>();
            torque.x = Random.Range(-200, 200);
            torque.y = Random.Range(-200, 200);
            torque.z = Random.Range(-200, 200);
            rb.AddTorque(torque);
            Vector3 v3 = (gM.player.transform.position - transform.position);
            v3.y = transform.position.y;
            rb.velocity =  v3* 500;

        }

    }


}
