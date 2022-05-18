using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAnimations : MonoBehaviour
{
    [SerializeField, Header("Will be set at runtime")]
    public Animator customerAnimator;
    public CustomerInteractable customerInteractable;
    Vector3 tempPosition;

    private NavMeshAgent navMeshAgent;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        customerAnimator = gameObject.GetComponentInChildren<Animator>();
        customerInteractable = gameObject.GetComponent<CustomerInteractable>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {

        HandleAnimations();
    }

    private void HandleAnimations()
    {
        speed = navMeshAgent.velocity.magnitude;
        customerAnimator.SetFloat("Speed",speed);
        
    }
    public void Talk()
    {
        customerAnimator.SetTrigger("Talking");
    }
}
