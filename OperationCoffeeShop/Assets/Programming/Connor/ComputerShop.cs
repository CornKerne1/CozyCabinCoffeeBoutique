using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComputerShop : MonoBehaviour
{

    PlayerMovement pm;
    PlayerCameraController pcc;

    GameMode gM;

    public float coffeePrice = 12;
    public float espressoPrice = 15;
    public float milkPrice = 7;
    public float sugarPrice = 10;

    [SerializeField] public static event EventHandler SpendMoney;

    private Queue<Ingredients> orders = new Queue<Ingredients>();

    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

        CoffeeBankTM.SuccessfulWithdrawl += EnsureWithdrawl;//
        pcc = gM.player.gameObject.GetComponent<PlayerCameraController>();
        pm = gM.player.gameObject.GetComponent<PlayerMovement>();
    }

    public void CloseShop()
    {
        Destroy(this.gameObject);
        pcc.canMove = true;
        pm.canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void BuyIngredient(string ingredient)
    {
        switch (ingredient)
        {
            case "Coffee":
                orders.Enqueue(Ingredients.UngroundCoffee);
                SpendMoney?.Invoke(coffeePrice, EventArgs.Empty);
                break;

            case "Milk":
                orders.Enqueue(Ingredients.Milk);
                SpendMoney?.Invoke(milkPrice, EventArgs.Empty);
                break;

            case "Espresso":
                orders.Enqueue(Ingredients.Espresso);
                SpendMoney?.Invoke(espressoPrice, EventArgs.Empty);
                break;

            case "Sugar":
                orders.Enqueue(Ingredients.Sugar);
                SpendMoney?.Invoke(sugarPrice, EventArgs.Empty);
                break;
        }
    }
    void EnsureWithdrawl(object sender, EventArgs e)
    {
        if ((bool)sender)
        {
            Debug.Log("adding + " + orders.Dequeue());
        }
        else
        {
            Debug.Log("Insufficent funds for + " + orders.Dequeue());
        }
    }
}
