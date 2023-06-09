using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public abstract class Machine : MonoBehaviour
{
    public int currentCapacity;
    [FormerlySerializedAs("mD")] public MachineData machineData;
    public IngredientAtlas iD;
    public bool isRunning;
    public Vector3 origin;
    [FormerlySerializedAs("GameMode")] public GameMode gameMode;

    public Transform outputTransform;
    [SerializeField] protected Transform cupTransform;
    private ObjectPool<GameObject> _pool;
    private int _i;
    private bool _takeCup;
    private IngredientContainer _cup;


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
    protected virtual async void OnTriggerEnter(Collider other)
    {
        if (!cupTransform||!other.TryGetComponent<IngredientContainer>(out var iC)) return;
        if (_takeCup)
        {
            await Task.Delay(1000);
            _takeCup = false;
            return;
        }
        _takeCup = true;
        _cup = iC;
        if(iC.playerInteraction)
            iC.playerInteraction.DropCurrentObj();
        else
            gameMode.player.GetComponent<PlayerInteraction>().DropCurrentObj();
        float elapsedTime = 0f;
        iC.transform.position = cupTransform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<IngredientContainer>(out var iC)) return;
        if(iC==_cup)
            _takeCup = true;
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

    public virtual async void StartMachine()
    {
        if (!isRunning)
        {
            await ActivateMachine(machineData.productionTime);
        }
    }


    protected virtual async Task ActivateMachine(float time)
    {
        isRunning = true;
        await Task.Delay(TimeSpan.FromSeconds(time));
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
    }
}