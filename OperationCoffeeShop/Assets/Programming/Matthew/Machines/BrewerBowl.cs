using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BrewerBowl : MonoBehaviour
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
    public GameMode gameMode;

    [FormerlySerializedAs("objectiveOutputObject1")] [SerializeField, Header("Tutorial stuff")]
    public GameObject objectiveOutputObject;

    public GameObject objectiveOutputObject1;

    private void Start()
    {
        _ieEnumerator = CO_ImitateUpdate();
        _machine = transform.root.GetComponentInChildren<Machine>();
        filter = filterTrans.gameObject;
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        CheckTutorial();
    }

    private void Update()
    {
        if (!_run) return;
        if (_ieEnumerator != null)
        {
            StartCoroutine(CO_ImitateUpdate());
        }

        IfTutorial2();
    }

    protected virtual void CheckTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            Debug.Log("Interactable tutorial object: " + gameObject);
            gameMode.Tutorial.AddedGameObject(gameObject);
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

            IfTutorial3();


            _run = false;
        }
        else
        {
            if (transform.position != openTrans.position) yield break;
            open = true;
            IfTutorial4();

            _run = false;
        }
    }

    public void IngredientInteract(GameObject other)
    {
        switch (open)
        {
            case true when filter.activeSelf:
                _machine.IngredientInteract(other);
                break;
            case true when other.GetComponent<PhysicalIngredient>().thisIngredient == Ingredients.CoffeeFilter:
                filter.SetActive(true);
                IfTutorial1();
                Destroy(other);
                break;
        }
    }

    public void RemoveFilter()
    {
        filter.SetActive(false);
    }

    private void IfTutorial1()
    {
        Debug.Log("tutorial1 with: " + objectiveOutputObject1);
        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(objectiveOutputObject1);
        }
    }

    private void IfTutorial2()
    {
        Debug.Log("tutorial2 with: " + gameObject);

        if (!filter.activeSelf && gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }

    private void IfTutorial3()
    {
        Debug.Log("tutorial3  with: " + objectiveOutputObject);

        if (filter.activeSelf && gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(objectiveOutputObject);
        }
    }

    private void IfTutorial4()
    {
        Debug.Log("tutorial4 with: " + gameObject);

        if (gameMode.gameModeData.inTutorial)
        {
            gameMode.Tutorial.NextObjective(gameObject);
        }
    }
}