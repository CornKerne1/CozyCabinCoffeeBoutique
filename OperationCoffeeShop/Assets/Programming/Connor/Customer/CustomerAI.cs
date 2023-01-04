using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.Serialization;

public class CustomerAI : MonoBehaviour
{
    private Vector3 _startingPosition;

    [SerializeField] private Vector3 destination;

    [SerializeField] public List<GameObject> destinations = new List<GameObject>();

    [FormerlySerializedAs("_destinationQueue")] [SerializeField]
    private Queue<Vector3> destinationQueue;

    public NavMeshAgent agent;
    [HideInInspector] public CustomerData customerData;

    [SerializeField] public bool stay = false;

    [SerializeField] public float minDistance = 1;

    [SerializeField] public List<CustomerLine> customerLines = new List<CustomerLine>();

    [SerializeField] public bool hasOrdered = false;
    [SerializeField] public bool hasOrder = false;
    [SerializeField] public bool lookAtBool = false;

    [SerializeField] public GameObject path;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(CO_Wait());
        StartCoroutine(CO_AddSelfToData());


        GameMode.ShopClosed += ShopClosed;
    }

    public void PathConditioning(GameObject givenPath)
    {
        if (destinationQueue != null)
        {
            Debug.LogWarning("Destination queue is not null when path conditioning.");
            return;
        }


        destinationQueue = new Queue<Vector3>();
        Debug.LogWarning("set Path");
        path = givenPath;
        Debug.Log("path is " + givenPath);
        destinations = new List<GameObject>();

        foreach (var dest in givenPath.GetComponentsInChildren<Transform>())
        {
            destinations.Add(dest.gameObject);
        }

        destinations.Remove(destinations[0]);
        foreach (var go in destinations)
        {
            destinationQueue.Enqueue(go.transform.position);
        }

        agent = gameObject.GetComponent<NavMeshAgent>();


        if (destination == Vector3.zero)
        {
            if (destinationQueue.Peek() != null)
            {
                destination = destinationQueue.Dequeue();
            }
        }

        setStay(false);
        customerLines = new List<CustomerLine>();
        hasOrder = false;
        hasOrdered = false;
    }

    private IEnumerator CO_Wait()
    {
        yield return new WaitForSeconds(.4f);
        customerData = gameObject.GetComponent<Customer>().customerData;
    }

    private void Update()
    {
        if (!path) return;
        if (!stay) //when not in line
        {
            lookAtBool = false;
            agent.destination = destination;
            var distanceToNode = gameObject.transform.position - destination;
            if (distanceToNode.magnitude >= minDistance) return;
            if (destinationQueue.Count == 0 && agent.hasPath == false && hasOrder && hasOrdered)
            {
                this.gameObject.SetActive(false);
                return;
            }

            destination = destinationQueue.Dequeue();
            setDestination(destination);
        }
        else if (lookAtBool == false) // when in line
        {
            if (!(agent.velocity.magnitude < 2f)) return;
            var lookat = this.customerLines[this.customerLines.Count - 1].gameObject.transform;
            var transform1 = transform;
            var direction = lookat.position - transform1.position;
            direction.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform1.rotation, Quaternion.LookRotation(direction),
                Time.time * .06f);
        }
    }

    IEnumerator CO_AddSelfToData()
    {
        yield return new WaitForSeconds(.45f);
        customerData.customerAI = this;
        if (path != null && destinationQueue == null)
        {
            PathConditioning(path);
        }
    }

    private IEnumerator Die(float death)
    {
        yield return new WaitForSeconds(death);
        DrinkData drink = new DrinkData("nothing");
        drink.price = 0;
        drink.ingredients = new List<IngredientNode>();
        if (hasOrdered)
            customerLines[customerLines.Count - 1].LeaveWithoutGettingDrink(drink);
        else
            customerLines[customerLines.Count - 1].LeaveWithoutPaying(drink);
        hasOrder = true;
        hasOrdered = true;
        var finalDestination = destinationQueue.ToArray()[(destinationQueue.ToArray().Length - 1)];
        destinationQueue.Clear();
        gameObject.GetComponent<CustomerInteractable>().dialogueManager.ExitDialogueMode();
        //gameObject.SetActive(false);// use at last resort
        setDestination(finalDestination);
        stay = false;
    }

    public void setDestination(Vector3 destination)
    {
        this.destination = destination;
        agent.SetDestination(destination);
    }

    public void queueDestination(Vector3 v)
    {
        destinationQueue ??= new Queue<Vector3>();

        destinationQueue.Enqueue(v);
    }

    public void ClearAndNullQueue()
    {
        destinationQueue = null;
    }

    public void setStay(bool stay)
    {
        this.stay = stay;
    }

    public void LeaveLine()
    {
        customerLines[customerLines.Count - 1].MoveLine();
    }

    public void ShopClosed(object sender, EventArgs e)
    {
        //CALLED WHEN THE SHOP CLOSES

        //CONNOR please write a method that makes the customers leave when the shop closes; Any drink not delivered will be scored as a zero! 
        if (this.isActiveAndEnabled)
            StartCoroutine(Die(10));
    }
}