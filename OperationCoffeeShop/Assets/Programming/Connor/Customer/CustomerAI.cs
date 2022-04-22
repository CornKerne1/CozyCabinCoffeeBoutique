using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private Vector3 startingPosition;

    [SerializeField]
    private Vector3 destination;

    public List<GameObject> Destinations = new List<GameObject>();

    private Queue<Vector3> dests = new Queue<Vector3>();

    public NavMeshAgent agent;
    [HideInInspector]
    public CustomerData CD;

    public bool stay = false;

    public float minDistance = 1;

    public List<CustomerLine> customerLines = new List<CustomerLine>();

    public bool hasOrdered = false;
    public bool hasOrder = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject path = GameObject.Find("Customer Path");
        foreach (Transform dest in path.GetComponentsInChildren<Transform>())
        {
            Destinations.Add(dest.gameObject);
        }
        Destinations.Remove(Destinations[0]);
        foreach (GameObject go in Destinations)
        {
            dests.Enqueue(go.transform.position);
        }
        agent = gameObject.GetComponent<NavMeshAgent>();

        this.CD = gameObject.GetComponent<Customer>().CD;
        if (destination == Vector3.zero)
        {
            if (dests.Peek() != null)
            {
                destination = dests.Dequeue();
            }
        }
        this.setStay(false);
        this.customerLines = new List<CustomerLine>();
        this.hasOrder = false;
        this.hasOrdered = false;



    }
    private void Update()
    {
        if (!stay)
        {
            agent.destination = destination;
            Vector3 distanceToNode = gameObject.transform.position - destination;
            if (distanceToNode.magnitude < minDistance && dests.Count != 0)
            {
                destination = dests.Dequeue();
                setDestination(destination);
            }
            else if (agent.hasPath ==false&& hasOrder &&hasOrdered)
            {
                this.gameObject.SetActive(false);
            }
        }
        

    }
    public void setDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);

    }
    public void queueDestination(Vector3 v)
    {
        dests.Enqueue(v);
    }

    public void clearQueue()
    {
        dests.Clear();
    }

    public void setStay(bool stay)
    {
        this.stay = stay;
    }

    public void LeaveLine()
    {
        
            customerLines[customerLines.Count-1].moveLine();
        
    }
}
