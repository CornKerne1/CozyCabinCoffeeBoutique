using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private Vector3 startingPosition;

    [SerializeField]
    private Vector3 destination;
   
    public List<GameObject> gameObjects = new List<GameObject>();
    
    public Queue<GameObject> dests = new Queue<GameObject>();

    public NavMeshAgent agent;
     [HideInInspector]
    public CustomerData CD;
    private void Awake()
    {
        foreach(GameObject go in gameObjects)
        {
            dests.Enqueue(go);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
       
            this.CD = gameObject.GetComponent<Customer>().CD;
        if (destination == Vector3.zero)
        {
            if (dests.Peek() != null)
            {
                destination = dests.Dequeue().transform.position;
            }
        }

    }
    private void Update()
    {
        
 
            Vector3 distanceToNode = gameObject.transform.position - destination;
            if (distanceToNode.magnitude < 3)
            {
                destination = dests.Dequeue().transform.position;
                setDestination(destination);
            }
        
    }
    public void setDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);

    }
}
