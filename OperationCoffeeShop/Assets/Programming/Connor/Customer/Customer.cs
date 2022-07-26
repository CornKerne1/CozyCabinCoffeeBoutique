using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public float DesiredQuality = .5f;

    [FormerlySerializedAs("ps")] [SerializeField]
    protected ParticleSystem particleSystem;

    private ParticleSystemRenderer _particleSystemRenderer;
    [SerializeField] protected Material like;
    [SerializeField] protected Material dislike;
    [SerializeField] protected Material surprise;
    [SerializeField] protected ParticleSystem surpriseParticleSystem;
    public float distanceToSurprise;

    private ObjectPool<ParticleSystem> _surprisePool;

    public virtual void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _particleSystemRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        customerData.customer = this;
        GameMode.SurpriseCustomers += OnSurprise;
        distanceToSurprise = 100;
        _surprisePool = new ObjectPool<ParticleSystem>(
            () => Instantiate(surpriseParticleSystem, gameObject.transform, false),
            particleSystem => { particleSystem.gameObject.SetActive(true); },
            particleSystem => { particleSystem.gameObject.SetActive(false); }, Destroy, true, 10, 10);
    }

    protected DrinkData GetFavoriteDrink()
    {
        if (customerData != null || customerData.name != null)
            Debug.Log("Null Favorite drink or CustomerData ");
        return customerData.favoriteDrinkData;
    }

    public abstract DrinkData GetDrinkOrder();

    public void OnReceivedDrink()
    {
        var quality =
            DrinkData.Compare(customerData.receivedDrinkData, customerData.orderedDrinkData);
        CustomerRating?.Invoke(quality, EventArgs.Empty);
        _particleSystemRenderer.material = quality > DesiredQuality ? like : dislike;
        AkSoundEngine.PostEvent(quality > DesiredQuality ? "PLAY_SATISFIEDCUSTOMER" : "PLAY_UNSATISFIEDCUSTOMER",
            gameObject);
        particleSystem.Play();
        Debug.Log("Drink Quality = " + quality);
    }

    protected void OnSurprise(object sender, EventArgs eventArgs)
    {
        var distance = Vector3.Distance(((GameObject)sender).transform.position, gameObject.transform.position);
        Debug.Log("play surprise with distance: " + distance);

        if (distance > distanceToSurprise) return;
        AkSoundEngine.PostEvent("PLAY_SURPRISE", gameObject);
        var particleSystem = _surprisePool.Get();
        particleSystem.gameObject.transform.localPosition = new Vector3(0, 1, 0);
        particleSystem.gameObject.transform.localRotation = new Quaternion(-90, 0, 0, 0);
        StartCoroutine(CO_SuppressSurprise(particleSystem));


        Debug.Log("play surprise");
    }

    private IEnumerator CO_SuppressSurprise(ParticleSystem particleSystem)
    {
        yield return new WaitForSeconds(1.5f);
        _surprisePool.Release(particleSystem);
    }
}