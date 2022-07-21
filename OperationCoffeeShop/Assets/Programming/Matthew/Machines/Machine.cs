using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Machine : MonoBehaviour
{
    public int currentCapacity;
    public MachineData mD;
    public IngredientData iD;
    public bool isRunning;
    public Vector3 origin;
    [FormerlySerializedAs("GameMode")] public GameMode gameMode;

    public Transform outputTransform;


    private void Awake()
    {
        origin = transform.position;
        mD.outputIngredient.Clear();
    }

    protected void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        CheckTutorial();
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
        if (currentCapacity < mD.maxCapacity && !isRunning)
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
            StartCoroutine(ActivateMachine(mD.productionTime));
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
        for (var i = 0; i < currentCapacity;)
            if (currentCapacity != 0)
            {
                currentCapacity = currentCapacity - 1;
                Debug.Log(currentCapacity);
                Instantiate(mD.outputIngredient[i], outputTransform.position, outputTransform.rotation);
                mD.outputIngredient.RemoveAt(i);
            }
    }


    protected virtual void Shake()
    {
        if (!isRunning) return;
        var shakePos = origin;
        shakePos.x = origin.x + Mathf.Sin(Time.time * mD.vibeSpeed) * mD.vibeAmt.x;
        shakePos.y = origin.y + Mathf.Sin(Time.time * mD.vibeSpeed) * mD.vibeAmt.y;
        shakePos.z = origin.z + Mathf.Sin(Time.time * mD.vibeSpeed) * mD.vibeAmt.z;
        transform.position = shakePos;
    }

    public void PostSoundEvent(string s)
    {
        AkSoundEngine.PostEvent(s, this.gameObject);
    }
}