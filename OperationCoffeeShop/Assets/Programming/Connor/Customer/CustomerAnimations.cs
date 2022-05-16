using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimations : MonoBehaviour
{
    [SerializeField, Header("Will be set at runtime")]
    public Animator customerAnimator;
    public Transform customerRigedBody;
    public CustomerInteractable customerInteractable;
    Vector3 tempPosition;


    // Start is called before the first frame update
    void Start()
    {
        customerAnimator = gameObject.GetComponentInChildren<Animator>();
        customerRigedBody = gameObject.GetComponent<Transform>();
        customerInteractable = gameObject.GetComponent<CustomerInteractable>();
        tempPosition = customerRigedBody.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        StartCoroutine(setTempPosition());
        if (customerRigedBody.transform.position != tempPosition)
        {
            customerAnimator.SetBool("Moving", true);
        }
        else customerAnimator.SetBool("Moving", false);
    }

    IEnumerator setTempPosition()
    {
        yield return new WaitForSeconds(1);
         tempPosition = customerRigedBody.transform.position;

    }
    public void Talk()
    {
        customerAnimator.SetTrigger("Talking");
    }
}
