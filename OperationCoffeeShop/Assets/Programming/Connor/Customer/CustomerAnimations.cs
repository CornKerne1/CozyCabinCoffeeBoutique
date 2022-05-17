using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAnimations : MonoBehaviour
{
    [SerializeField, Header("Will be set at runtime")]
    public Animator customerAnimator;
    public Rigidbody customerRigedBody;
    public CustomerInteractable customerInteractable;

    // Start is called before the first frame update
    void Start()
    {
        customerAnimator = gameObject.GetComponentInChildren<Animator>();
        customerRigedBody = gameObject.GetComponent<Rigidbody>();
        customerInteractable = gameObject.GetComponent<CustomerInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        customerAnimator.SetFloat("Speed", GetComponent<NavMeshAgent>().velocity.magnitude);
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 0)
        {
            customerAnimator.SetBool("Moving", true);
        }
        else
        {
            customerAnimator.SetBool("Moving", false);
            customerAnimator.SetFloat("Speed", .75f);
        }
    }
    public void Talk()
    {
        customerAnimator.SetTrigger("Talking");
    }
}
