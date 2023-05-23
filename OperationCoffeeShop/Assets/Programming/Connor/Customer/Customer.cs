using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public abstract class Customer : MonoBehaviour
{
    public RandomConversations randomConversations;

    [Header("RandomCustomer will be empty")]
    public CustomerData customerData;

    public PlayerResearchData playerResearchData;
    [HideInInspector] public GameMode gameMode;

    public static event EventHandler CustomerRating;

    [FormerlySerializedAs("DesiredQuality")]
    public float desiredQuality = .5f;

    [FormerlySerializedAs("ps")] [SerializeField]
    protected new ParticleSystem particleSystem;

    private ParticleSystemRenderer _particleSystemRenderer;
    [SerializeField] protected Material like;
    [SerializeField] protected Material dislike;
    [SerializeField] protected Material surprise;
    [SerializeField] protected ParticleSystem surpriseParticleSystem;
    public float distanceToSurprise = 10;

    private ObjectPool<ParticleSystem> _surprisePool;

    public virtual void Awake()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        customerData.customer = this;
        GameMode.SurpriseCustomers += OnSurprise;
        _surprisePool = new ObjectPool<ParticleSystem>(
            () => Instantiate(surpriseParticleSystem, gameObject.transform, false),
            system =>
            {
                system.gameObject.SetActive(true);
                system.Play();
            },
            system => { system.gameObject.SetActive(false); }, Destroy, true, 10, 10);
    }

    protected DrinkData GetFavoriteDrink()
    {
        return customerData.favoriteDrinkData;
    }

    public abstract DrinkData GetDrinkOrder();

    public void OnReceivedDrink()
    {
        _particleSystemRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();

        var quality =
            DrinkData.Compare(customerData.receivedDrinkData, customerData.orderedDrinkData);
        CustomerRating?.Invoke(quality, EventArgs.Empty);
        Debug.Log(_particleSystemRenderer);
        Debug.Log(_particleSystemRenderer.material);

        Debug.Log(like);

        _particleSystemRenderer.material = like;
        isGoodDrink();
        _particleSystemRenderer.material = dislike;

        _particleSystemRenderer.material = isGoodDrink() ? like : dislike;
        AkSoundEngine.PostEvent(isGoodDrink() ? "PLAY_SATISFIEDCUSTOMER" : "PLAY_UNSATISFIEDCUSTOMER",
            gameObject);
        particleSystem.Play();
        Debug.Log("Drink Quality = " + quality);
    }

    public bool isGoodDrink()
    {
        var quality =
            DrinkData.Compare(customerData.receivedDrinkData, customerData.orderedDrinkData);
        return quality > desiredQuality;
    }

    private void OnSurprise(object sender, EventArgs eventArgs)
    {
        var distance = Vector3.Distance(((GameObject)sender).transform.position, gameObject.transform.position);
        Debug.Log("play surprise with distance: " + distance);

        if (distance > distanceToSurprise) return;
        AkSoundEngine.PostEvent("PLAY_SURPRISE", gameObject);
        var system = _surprisePool.Get();
        var o = system.gameObject;
        o.transform.localPosition = new Vector3(0, 1, 0);
        o.transform.localRotation = new Quaternion(-90, 0, 0, 0);
        try
        {
            StartCoroutine(CO_SuppressSurprise(system));
        }
        catch
        {
            // ignored
        }


        Debug.Log("play surprise");
    }

    private IEnumerator CO_SuppressSurprise(ParticleSystem system)
    {
        yield return new WaitForSeconds(1.5f);
        _surprisePool.Release(system);
    }
}