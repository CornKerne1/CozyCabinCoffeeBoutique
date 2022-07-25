using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public abstract class Machine : MonoBehaviour
{
    public int currentCapacity;
    [FormerlySerializedAs("mD")] public MachineData machineData;
    public IngredientData iD;
    public bool isRunning;
    public Vector3 origin;
    [FormerlySerializedAs("GameMode")] public GameMode gameMode;

    public Transform outputTransform;

    private ObjectPool<GameObject> _pool;
    private int _i;

    private void Awake()
    {
        origin = transform.position;
        machineData.outputIngredient.Clear();
    }

    protected void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        CheckTutorial();
        _pool = new ObjectPool<GameObject>(() =>
            {
                var gameObject = Instantiate(machineData.outputIngredient[_i], outputTransform.position,
                    outputTransform.rotation);
                gameObject.GetComponent<PhysicalIngredient>().machine = this;
                return gameObject;
            }, o =>
            {
                o.SetActive(true);
                o.transform.position = outputTransform.position;
                o.transform.rotation = outputTransform.rotation;
            },
            gameObject => { gameObject.SetActive(false); }, Destroy, true, 100, 100);
    }

    private void Update()
    {
        Shake();
    }

    protected virtual void CheckTutorial()
    {
        if (gameMode.gameModeData.inTutorial)
        {
            Debug.Log("Interactable tutorial object: " + gameObject);
            gameMode.Tutorial.AddedGameObject(gameObject);
        }
    }

    public void IngredientInteract(GameObject other)
    {
        if (currentCapacity < machineData.maxCapacity && !isRunning)
        {
            ChooseIngredient(other);
        }
    }

    protected virtual void ChooseIngredient(GameObject other)
    {
        //switch (other.GetComponent<PhysicalIngredient>().thisIngredient)
        //{
        //    case Ingredients.LightRoastCoffee:                

        //        currentCapacity = currentCapacity + 1;
        //        mD.outputIngredient.Add(iD.glCoffee);
        //        other.GetComponent<PhysicalIngredient>().pI.DropCurrentObj();
        //        Destroy(other);
        //        break;
        //}
    }

    public virtual void StartMachine()
    {
        if (!isRunning)
        {
            StartCoroutine(ActivateMachine(machineData.productionTime));
        }
    }


    protected virtual IEnumerator ActivateMachine(float time)
    {
        isRunning = true;
        yield return new WaitForSeconds(time);
        OutputIngredients();
        transform.position = origin;
        isRunning = false;
    }

    protected virtual void OutputIngredients()
    {
        for (_i = 0; _i < currentCapacity;)
            if (currentCapacity != 0)
            {
                currentCapacity--;
                _pool.Get();
                machineData.outputIngredient.RemoveAt(_i);
            }
    }


    protected virtual void Shake()
    {
        if (!isRunning) return;
        var shakePos = origin;
        shakePos.x = origin.x + Mathf.Sin(Time.time * machineData.vibeSpeed) * machineData.vibeAmt.x;
        shakePos.y = origin.y + Mathf.Sin(Time.time * machineData.vibeSpeed) * machineData.vibeAmt.y;
        shakePos.z = origin.z + Mathf.Sin(Time.time * machineData.vibeSpeed) * machineData.vibeAmt.z;
        transform.position = shakePos;
    }

    public void PostSoundEvent(string s)
    {
        AkSoundEngine.PostEvent(s, this.gameObject);
    }

    public void ReleasePoolObject(GameObject obj)
    {
        _pool.Release(obj);
        Debug.Log("We release you coffee bean");
    }
}