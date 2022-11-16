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


    private Queue<DeliveryManager.ObjType> _orders;

    // Start is called before the first frame update
    private void Start()
    {
        _orders = new Queue<DeliveryManager.ObjType>();

        _gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        coffeeBankTM = _gameMode.CoffeeBankTM;
        CoffeeBankTM.SuccessfulWithdrawal += EnsureWithdrawal; //
        balance.text = balanceString + _gameMode.gameModeData.moneyInBank;
        bankUpdate.text = "";
    }

    public void CloseShop()
    {
        _orders = new Queue<DeliveryManager.ObjType>();
        _gameMode.playerData.canMove = true;
        _gameMode.playerData.canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        this.gameObject.SetActive(false);
    }

    public void BuyIngredient(string i)
    {
        DeliveryManager.ObjType.TryParse<DeliveryManager.ObjType>(i,out var ingredient);
        _orders.Enqueue(ingredient);
        switch (ingredient)
        {
            case DeliveryManager.ObjType.Coffee:
                SpendMoney?.Invoke(coffeePrice, EventArgs.Empty);
                break;

            case DeliveryManager.ObjType.Milk:
                SpendMoney?.Invoke(milkPrice, EventArgs.Empty);
                break;

            case DeliveryManager.ObjType.Espresso:
                SpendMoney?.Invoke(espressoPrice, EventArgs.Empty);
                break;

            case DeliveryManager.ObjType.Sugar:
                SpendMoney?.Invoke(sugarPrice, EventArgs.Empty);
                break;
        }
    }

    private void EnsureWithdrawal(object sender, EventArgs e)
    {
        if (_orders.Count <= 0) return;
        if ((bool)sender)
        {
            balance.text = balanceString + _gameMode.gameModeData.moneyInBank;
            bankUpdate.color = Color.green;
            var ingredient = _orders.Dequeue();
            bankUpdate.text = bankSuccessString + ingredient;

            switch (ingredient)
            {
                case DeliveryManager.ObjType.Coffee:
                    _gameMode.DeliveryManager.AddToDelivery(new DeliveryPackage(ingredient, coffeeQuantity));
                    break;

                case DeliveryManager.ObjType.Milk:
                    _gameMode.DeliveryManager.AddToDelivery(new DeliveryPackage(ingredient, milkQuantity));
                    break;

                case DeliveryManager.ObjType.Espresso:
                    _gameMode.DeliveryManager.AddToDelivery(new DeliveryPackage(ingredient, espressoQuantity));
                    break;

                case DeliveryManager.ObjType.Sugar:
                    _gameMode.DeliveryManager.AddToDelivery(new DeliveryPackage(ingredient, sugarQuantity));
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