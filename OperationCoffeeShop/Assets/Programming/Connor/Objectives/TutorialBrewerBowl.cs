using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialBrewerBowl : MonoBehaviour
{
    public bool open;
    private bool _run;

    [FormerlySerializedAs("OpenTrans")] [SerializeField]
    private Transform openTrans;

    [FormerlySerializedAs("CloseTrans")] [SerializeField]
    private Transform closeTrans;

    [SerializeField] private Transform filterTrans;
    public GameObject filter;
    private IEnumerator _ieEnumerator;
    private Machine _machine;
    private Objectives1 _objectives1;
    public GameObject objectiveOutputObject;


    private void Start()
    {
        _ieEnumerator = CO_ImitateUpdate();
        _machine = transform.root.GetComponentInChildren<Machine>();
        filter = filterTrans.gameObject;
        _objectives1 = GameObject.Find("Objectives").GetComponent<Objectives1>();
    }

    private void Update()
    {
        if (!_run) return;
        if (_ieEnumerator != null)
        {
            StartCoroutine(CO_ImitateUpdate());
        }

        if (filter.activeSelf)
        {
            _objectives1.NextObjective(objectiveOutputObject);
        }
        
    }

    public void OpenOrClose()
    {
        if (!_run)
        {
            _run = true;
        }
    }

    private IEnumerator CO_ImitateUpdate()
    {
        if (open)
        {
            Transform transform1;
            (transform1 = transform).position =
                Vector3.MoveTowards(transform.position, closeTrans.position, Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform1.rotation, closeTrans.rotation, .1f);
        }
        else
        {
            Transform transform1;
            (transform1 = transform).position =
                Vector3.MoveTowards(transform.position, openTrans.position, Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform1.rotation, openTrans.rotation, .1f);
        }

        yield return new WaitForSeconds(.02f * 10);
        if (open)
        {
            if (transform.position != closeTrans.position) yield break;
            open = false;
            _run = false;
        }
        else
        {
            if (transform.position != openTrans.position) yield break;
            open = true;
            _objectives1.NextObjective(gameObject);
            _run = false;
        }
    }

    public void IngredientInteract(GameObject other)
    {
        switch (open)
        {
            case true when filter.activeSelf:
                Debug.Log("this is a test");
                _machine.IngredientInteract(other);
                break;
            case true when other.GetComponent<PhysicalIngredient>().thisIngredient == Ingredients.CoffeeFilter:
                filter.SetActive(true);
                Debug.Log("fitler has landed");
                _objectives1.NextObjective(objectiveOutputObject);
                Destroy(other);
                break;
        }
    }
}