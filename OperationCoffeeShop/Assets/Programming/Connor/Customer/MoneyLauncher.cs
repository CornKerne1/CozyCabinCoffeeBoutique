using System.Collections.Generic;
using UnityEngine;

public class MoneyLauncher : MonoBehaviour
{
    private GameMode _gameMode;

    public GameObject dollarBill;
    public GameObject coin;

    // Start is called before the first frame update
    private void Start()
    {
        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    public void LaunchMoney(int dollars, int change)
    {
        Debug.Log("dollars:" + dollars + " cents: " + change);
        List<GameObject> money = new List<GameObject>();
        while (dollars-- > 0 && dollarBill != null)
        {
            var transform1 = transform;
            GameObject go = Instantiate(dollarBill, transform1.position, transform1.rotation);
            money.Add(go);
        }

        while (change-- > 0 && coin != null)
        {
            GameObject go = Instantiate(coin);
            money.Add(go);
        }

        foreach (GameObject m in money)
        {
            Vector3 torque = new Vector3();
            Rigidbody rb = m.GetComponent<Rigidbody>();
            torque.x = Random.Range(-200, 200);
            torque.y = Random.Range(-200, 200);
            torque.z = Random.Range(-200, 200);
            rb.AddTorque(torque);
            var position = transform.position;
            Vector3 v3 = (_gameMode.player.transform.position - position);
            v3.y = position.y;
            rb.velocity = v3 * 500;
        }
    }
}