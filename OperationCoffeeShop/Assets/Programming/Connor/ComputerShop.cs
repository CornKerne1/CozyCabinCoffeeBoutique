using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ComputerShop : MonoBehaviour
{

    PlayerMovement pm;
    PlayerCameraController pcc;

    GameMode gM;
    CoffeeBankTM cBTM;

    public TextMeshProUGUI balance;
    public string balanceString;
    public TextMeshProUGUI bankUpdate;
    public string bankSuccessString;
    public string bankFailureString;



    public float coffeePrice = 12;
    public int coffeeQuanitiy = 15;
    public ObjectHolder coffeeType;
    public float espressoPrice = 15;
    public int espressoQuantity = 15;
    public ObjectHolder espressoType;
    public float milkPrice = 7;
    public int milkQuantity = 10;
    public float sugarPrice = 10;
    public int sugarQuantity = 25;

    [SerializeField] public static event EventHandler SpendMoney;
    [SerializeField] public static event EventHandler DepositItems;//


    private Queue<string> orders;

    // Start is called before the first frame update
    void Start()
    {
        orders = new Queue<string>();

        gM = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        cBTM = gM.gameObject.GetComponent<CoffeeBankTM>();
        cBTM.ResetEvents();
        CoffeeBankTM.SuccessfulWithdrawl += EnsureWithdrawl;//
        pcc = gM.player.gameObject.GetComponent<PlayerCameraController>();
        pm = gM.player.gameObject.GetComponent<PlayerMovement>();
        balance.text = balanceString + cBTM.moneyInBank;
        bankUpdate.text = "";
    }

    public void CloseShop()
    {
        orders = new Queue<string>();
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
                orders.Enqueue("Coffee");
                SpendMoney?.Invoke(coffeePrice, EventArgs.Empty);
                break;

            case "Milk":
                orders.Enqueue("Milk");
                SpendMoney?.Invoke(milkPrice, EventArgs.Empty);
                break;

            case "Espresso":
                orders.Enqueue("Espresso");
                SpendMoney?.Invoke(espressoPrice, EventArgs.Empty);
                break;

            case "Sugar":
                orders.Enqueue("Sugar");
                Debug.Log("buyingredient sugar " + orders.Count);

                SpendMoney?.Invoke(sugarPrice, EventArgs.Empty);
                break;
        }
    }
    void EnsureWithdrawl(object sender, EventArgs e)
    {
        Debug.Log("ensureWithdrawl " + orders.Count);
        if ((bool)sender)
        {
            balance.text = balanceString + cBTM.moneyInBank;
            bankUpdate.color = Color.green;
            string ingredient = orders.Dequeue();
            bankUpdate.text = bankSuccessString + ingredient;
            
            switch (ingredient)
            {
                case "Coffee":
                    Tuple<ObjectHolder, int> coffee = new Tuple<ObjectHolder, int>(coffeeType, coffeeQuanitiy);
                    DepositItems?.Invoke(coffee, EventArgs.Empty);
                    break;

                case "Milk":
                    //DepositItems?.Invoke((milkQuantity), EventArgs.Empty);
                    break;

                case "Espresso":
                    Tuple<ObjectHolder, int> espresso = new Tuple<ObjectHolder, int>(espressoType, espressoQuantity);
                    DepositItems?.Invoke(espresso, EventArgs.Empty);
                    break;

                case "Sugar":
                    //DepositItems?.Invoke((sugarQuantity), EventArgs.Empty);
                    break;
            }


        }
        else
        {
            bankUpdate.color = Color.red;
            bankUpdate.text = bankFailureString + orders.Dequeue();
        }
    }

}
