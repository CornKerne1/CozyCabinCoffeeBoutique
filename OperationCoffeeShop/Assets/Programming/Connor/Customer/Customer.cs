using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Customer : MonoBehaviour
{
    public RandomConversations randomConversations;


    [Header("RandomCustomer will be empty")]
    public CustomerData customerData;

    public PlayerResearchData playerResearchData;
    [HideInInspector] public GameMode gameMode;

    [SerializeField] public static event EventHandler CustomerRating;

    [SerializeField] protected ParticleSystem ps;
    private ParticleSystemRenderer _particleSystemRenderer;
    [SerializeField] protected Material like;
    [SerializeField] protected Material dislike;

    public virtual void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        _particleSystemRenderer = ps.GetComponent<ParticleSystemRenderer>();
        customerData.customer = this;
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
        _particleSystemRenderer.material = quality > .5 ? like : dislike;
        AkSoundEngine.PostEvent(quality > .5f ? "PLAY_SATISFIEDCUSTOMER" : "PLAY_UNSATISFIEDCUSTOMER", gameObject);
        ps.Play();
        Debug.Log("Drink Quality = " + quality);
    }
}