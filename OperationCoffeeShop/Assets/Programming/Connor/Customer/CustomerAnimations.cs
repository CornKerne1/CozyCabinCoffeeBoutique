using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (customerRigedBody.velocity.magnitude > .1)
        {
            customerAnimator.SetBool("Moving", true);
        }
        else customerAnimator.SetBool("Moving", false);
    }
    public void Talk()
    {
        customerAnimator.SetTrigger("Talking");
    }
}
