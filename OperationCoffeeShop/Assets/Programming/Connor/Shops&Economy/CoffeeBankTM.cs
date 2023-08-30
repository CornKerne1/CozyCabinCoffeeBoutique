using UnityEngine;
using System;

public class CoffeeBankTM
{
    public CoffeeBankTM(CoffeeBankTM coffeeBankTM, GameMode gameMode,GameModeData gameModeData)
    {
        _coffeeBankTM = coffeeBankTM;
        _gameMode = gameMode;
        _gameModeData = gameModeData;
        CustomerLine.DepositMoney += DepositMoneyInBank;
        CarnivalTruck.DepositMoney += DepositMoneyInBank;
        ComputerShop.SpendMoney += WithdrawMoneyInBank;
    }
    public static event EventHandler SuccessfulWithdrawal;
    private CoffeeBankTM _coffeeBankTM;
    private GameMode _gameMode;
    private GameModeData _gameModeData;

    private void DepositMoneyInBank(object sender, EventArgs e)
    {
        float depositAmount = Convert.ToSingle(sender);
        _gameModeData.moneyInBank += depositAmount;
    }
    public void DepositMoneyInBank(float depositAmount)
    {
        _gameModeData.moneyInBank += depositAmount;
    }

    private void WithdrawMoneyInBank(object sender, EventArgs e)
    {
        if (sender is int || sender is float)
        {
            float withdrawalAmount = (float)Convert.ToDouble(sender);

            if (_gameModeData.moneyInBank - withdrawalAmount >= 0)
            {
                _gameModeData.moneyInBank -= withdrawalAmount;
                SuccessfulWithdrawal?.Invoke(true, EventArgs.Empty);
                Debug.Log("withdrawing money, money in bank now =" +  _gameModeData.moneyInBank);
            }
            else
            {
                SuccessfulWithdrawal?.Invoke(false, EventArgs.Empty);
            }
        }
        else
        {
            Debug.LogError("Cannot withdraw money. Invalid data type: " + sender.GetType());
        }
    }

    public void OnDestroy()
    {
        CustomerLine.DepositMoney -= DepositMoneyInBank;
        CarnivalTruck.DepositMoney -= DepositMoneyInBank;
        ComputerShop.SpendMoney -= WithdrawMoneyInBank;
    }
}