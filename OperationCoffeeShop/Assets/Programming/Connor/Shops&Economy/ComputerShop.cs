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
    public float coffeePrice = 12, espressoPrice = 15,milkPrice = 7,sugarPrice = 10;
    public int coffeeQuantity = 15,espressoQuantity = 15,milkQuantity = 10,sugarQuantity = 25;
    public ObjectHolder coffeeType,espressoType,sugarType;


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
        _gameMode.playerData.canMove = true;
        _gameMode.playerData.canMove = true;
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
                    _gameMode.DeliveryManager.AddToDelivery(new DeliveryPackage("Coffee", coffeeQuantity ));
                    break;

                case "Milk":
                    _gameMode.DeliveryManager.AddToDelivery(new DeliveryPackage("Milk", milkQuantity ));
                    break;

                case "Espresso":
                    _gameMode.DeliveryManager.AddToDelivery(new DeliveryPackage("Espresso", espressoQuantity));
                    break;

                case "Sugar":
                    _gameMode.DeliveryManager.AddToDelivery(new DeliveryPackage("Sugar", sugarQuantity));
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