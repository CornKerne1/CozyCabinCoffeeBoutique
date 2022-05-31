using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanomizeAnimations : MonoBehaviour
{
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animator>();
    }

    public void RandomIdle()
    {
        if (animator.IsInTransition(0) == false)
        {

        }
    }
}
