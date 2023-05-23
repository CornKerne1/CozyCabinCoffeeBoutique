using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.Serialization;

public class ComputerShop : MonoBehaviour
{
    private GameMode _gameMode;
    public CoffeeBankTM coffeeBankTM;

    private TextMeshProUGUI _balance,_bankUpdate;
    public GameObject balance,bankUpdate;
    public string balanceString;
    public string bankSuccessString;
    public string bankFailureString;
    public float coffeePrice = 12, espressoPrice = 15,milkPrice = 7,sugarPrice = 10,cameraPrice = 100,pictureFramePrice=15;
    public int coffeeQuantity = 15,espressoQuantity = 15,sugarQuantity = 25;
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
        _balance=balance.GetComponent<TextMeshProUGUI>();
        _bankUpdate=bankUpdate.GetComponent<TextMeshProUGUI>();
        _balance.text = balanceString + _gameMode.gameModeData.moneyInBank;
        _bankUpdate.text = "";
    }

    public void CloseShop()
    {
        _orders = new Queue<DeliveryManager.ObjType>();
        _gameMode.playerInput.ToggleMovement();
        _gameMode.playerData.inUI = false;
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
            case DeliveryManager.ObjType.Camera:
                SpendMoney?.Invoke(cameraPrice, EventArgs.Empty);
                break;
            case DeliveryManager.ObjType.PictureFrame:
                SpendMoney?.Invoke(pictureFramePrice, EventArgs.Empty);
                break;
        }
    }

    private void EnsureWithdrawal(object sender, EventArgs e)
    {
        if (_orders.Count <= 0) return;
        if ((bool)sender)
        {
            _balance.text = balanceString + _gameMode.gameModeData.moneyInBank;
            _bankUpdate.color = Color.green;
            var ingredient = _orders.Dequeue();
            _bankUpdate.text = bankSuccessString + ingredient.ToString();

            switch (ingredient)
            {
                case DeliveryManager.ObjType.Coffee:
                    _gameMode.deliveryManager.AddToDelivery(new DeliveryPackage(ingredient, coffeeQuantity));
                    break;

                case DeliveryManager.ObjType.Milk:
                    _gameMode.deliveryManager.AddToDelivery(new DeliveryPackage(ingredient, 0));
                    break;

                case DeliveryManager.ObjType.Espresso:
                    _gameMode.deliveryManager.AddToDelivery(new DeliveryPackage(ingredient, espressoQuantity));
                    break;

                case DeliveryManager.ObjType.Sugar:
                    _gameMode.deliveryManager.AddToDelivery(new DeliveryPackage(ingredient, sugarQuantity));
                    break;
                case DeliveryManager.ObjType.Camera:
                    _gameMode.deliveryManager.AddToDelivery(new DeliveryPackage(ingredient, 0));
                    break;
                case DeliveryManager.ObjType.PictureFrame:
                    _gameMode.deliveryManager.AddToDelivery(new DeliveryPackage(ingredient, 0));
                    break;
            }
        }
        else
        {
            _bankUpdate.color = Color.red;
            _bankUpdate.text = bankFailureString + _orders.Dequeue();
        }
    }

    public void PlayMouseClick()
    {
        AkSoundEngine.PostEvent("PLAY_MOUSECLICK", gameObject);
    }
}