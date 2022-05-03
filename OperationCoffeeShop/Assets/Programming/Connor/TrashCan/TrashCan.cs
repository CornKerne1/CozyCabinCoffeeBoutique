using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{

    Collider leverCollider;

    Animator trashAnimator;

    private void Start()
    {
        this.trashAnimator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           if (this.trashAnimator.GetBool("Open"))
            {

            }
           else
            {
                this.trashAnimator.SetBool("Open", true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!this.trashAnimator.GetBool("Open"))
            {

            }
            else
            {
                this.trashAnimator.SetBool("Open", false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (this.trashAnimator.GetBool("Open"))
            {

            }
            else
            {
                this.trashAnimator.SetBool("Open", true);
            }
        }
    }
}
