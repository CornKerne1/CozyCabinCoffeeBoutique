using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Serialization;

public class ComputerShop : MonoBehaviour
{
    private GameMode _gameMode;
    public CoffeeBankTM coffeeBankTM;

    public TextMeshProUGUI balance;
    public string balanceString;
    public TextMeshProUGUI bankUpdate;
    public string bankSuccessString;
    public string bankFailureString;


    public float coffeePrice = 12;

    [FormerlySerializedAs("coffeeQuanitiy")]
    public int coffeeQuantity = 15;

    public ObjectHolder coffeeType;
    public float espressoPrice = 15;
    public int espressoQuantity = 15;
    public ObjectHolder espressoType;
    public float milkPrice = 7;
    public int milkQuantity = 10;
    public float sugarPrice = 10;
    public int sugarQuantity = 25;
    public ObjectHolder sugarType;


    public static event EventHandler SpendMoney;
    public static event EventHandler DepositItems;


    private Queue<string> _orders;

    // Start is called before the first frame update
    private void Start()
    {
        _orders = new Queue<string>();

        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        coffeeBankTM = _gameMode.gameObject.GetComponent<CoffeeBankTM>();
        CoffeeBankTM.SuccessfulWithdrawal += EnsureWithdrawal; //
        balance.text = balanceString + coffeeBankTM.moneyInBank;
        bankUpdate.text = "";
    }

    public void CloseShop()
    {
        _orders = new Queue<string>();
        _gameMode.pD.canMove = true;
        _gameMode.pD.canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.gameObject.SetActive(false);
    }

    public void BuyIngredient(string ingredient)
    {
        switch (ingredient)
        {
            case "Coffee":
                _orders.Enqueue("Coffee");
                SpendMoney?.Invoke(coffeePrice, EventArgs.Empty);
                break;

            case "Milk":
                _orders.Enqueue("Milk");
                SpendMoney?.Invoke(milkPrice, EventArgs.Empty);
                break;

            case "Espresso":
                _orders.Enqueue("Espresso");
                SpendMoney?.Invoke(espressoPrice, EventArgs.Empty);
                break;

            case "Sugar":
                _orders.Enqueue("Sugar");
                Debug.Log("buy ingredient sugar " + _orders.Count);

                SpendMoney?.Invoke(sugarPrice, EventArgs.Empty);
                break;
        }
    }

    private void EnsureWithdrawal(object sender, EventArgs e)
    {
        if (_orders.Count <= 0) return;
        if ((bool)sender)
        {
            balance.text = balanceString + coffeeBankTM.moneyInBank;
            bankUpdate.color = Color.green;
            var ingredient = _orders.Dequeue();
            bankUpdate.text = bankSuccessString + ingredient;

            switch (ingredient)
            {
                case "Coffee":
                    Tuple<ObjectHolder, int> coffee = new Tuple<ObjectHolder, int>(coffeeType, coffeeQuantity);
                    DepositItems?.Invoke(coffee, EventArgs.Empty);
                    break;

                case "Milk":
                    Tuple<Ingredients, int> milk = new Tuple<Ingredients, int>(Ingredients.Milk, milkQuantity);
                    DepositItems?.Invoke(milk, EventArgs.Empty);
                    break;

                case "Espresso":
                    Tuple<ObjectHolder, int> espresso = new Tuple<ObjectHolder, int>(espressoType, espressoQuantity);
                    DepositItems?.Invoke(espresso, EventArgs.Empty);
                    break;

                case "Sugar":
                    Tuple<ObjectHolder, int> sugar = new Tuple<ObjectHolder, int>(sugarType, sugarQuantity);
                    DepositItems?.Invoke(sugar, EventArgs.Empty);
                    break;
            }
        }
        else
        {
            bankUpdate.color = Color.red;
            bankUpdate.text = bankFailureString + _orders.Dequeue();
        }
    }

    public void PlayMouseClick()
    {
        AkSoundEngine.PostEvent("PLAY_MOUSECLICK", gameObject);
    }
}