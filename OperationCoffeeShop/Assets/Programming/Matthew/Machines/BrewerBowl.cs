using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class BrewerBowl : MonoBehaviour
{
    public bool open = false;
    private bool run = false;
    [SerializeField] private Transform OpenTrans;
    [SerializeField] private Transform CloseTrans;
    [SerializeField] private Transform filterTrans;
    public GameObject filter;
    private IEnumerator iU;
    private Machine m;

    private void Start()
    {
        iU = ImitateUpdate();
        m = transform.root.GetComponentInChildren<Machine>();
        filter = filterTrans.gameObject;
    }

    private void Update()
    {
        if (run)
        {
            if (iU != null)
            {
                StartCoroutine(ImitateUpdate());
            }
        }
    }

    public void OpenOrClose()
    {
        if (!run)
        {
            run = true;
        }
    }

    public IEnumerator ImitateUpdate()
    {
        if (open)
        {
            transform.position = Vector3.MoveTowards(transform.position, CloseTrans.position, Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, CloseTrans.rotation, .1f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, OpenTrans.position, Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, OpenTrans.rotation, .1f);
        }
        yield return new WaitForSeconds(.02f * 10);
        if (open)
        {
            if (transform.position == CloseTrans.position)
            {
                open = false;
                run = false;
            }
        }
        else
        {
            if (transform.position == OpenTrans.position)
            {
                open = true;
                run = false;
            }
        }
    }
    
    public void IngredientInteract(GameObject other)
    {
        if (open && filter.activeSelf)
        {
            m.IngredientInteract(other);
        }
        else if(open && other.GetComponent<PhysicalIngredient>().thisIngredient == Ingredients.CoffeeFilter)
        {
            filter.SetActive(true);
            Destroy(other);
        }
    }


}
