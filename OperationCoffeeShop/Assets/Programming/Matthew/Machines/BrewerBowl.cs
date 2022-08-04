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

    [SerializeField, Header("Tutorial stuff")]
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

            IfTutorial(3);


            _run = false;
        }
        else
        {
            if (transform.position != openTrans.position) yield break;
            open = true;
            IfTutorial(2);

            _run = false;
        }
    }

    public void IngredientInteract(GameObject other)
    {
        var physicalIngredient = other.GetComponent<PhysicalIngredient>();
        switch (open)
        {
            case true when filter.activeSelf:
                _machine.IngredientInteract(other);
                break;
            case true when physicalIngredient.thisIngredient == Ingredients.CoffeeFilter:
                filter.SetActive(true);
                IfTutorial(1);
                physicalIngredient.dispenser.ReleasePoolObject(physicalIngredient);
                break;
        }
    }

    private void IfTutorial(int i)
    {
        switch (i)
        {
            case 1:
                if (gameMode.gameModeData.inTutorial)
                {
                    gameMode.Tutorial.NextObjective(objectiveOutputObject1);
                }

                break;
            case 2:
                if (gameMode.gameModeData.inTutorial)
                {
                    gameMode.Tutorial.NextObjective(gameObject);
                }

                break;
            case 3:
                if (filter.activeSelf && gameMode.gameModeData.inTutorial)
                {
                    gameMode.Tutorial.NextObjective(objectiveOutputObject);
                }

                break;
        }
    }
}