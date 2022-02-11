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

    public Queue<Vector3> dests = new Queue<Vector3>();

    public NavMeshAgent agent;
    [HideInInspector]
    public CustomerData CD;

    // Start is called before the first frame update
    void Start()
    {
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

    }
    private void Update()
    {
        agent.destination = destination;
        Vector3 distanceToNode = gameObject.transform.position - destination;
        if (distanceToNode.magnitude < 3 && dests.Count != 0)
        {
            destination = dests.Dequeue();
            setDestination(destination);
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
}
