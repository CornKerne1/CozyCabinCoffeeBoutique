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
        ComputerShop.SpendMoney += WithdrawMoneyInBank;
    }
    public static event EventHandler SuccessfulWithdrawal;
    private CoffeeBankTM _coffeeBankTM;
    private GameMode _gameMode;
    private GameModeData _gameModeData;

    private void DepositMoneyInBank(object sender, EventArgs e)
    {
        try
        {
            _gameModeData.moneyInBank += (float)sender;
        }
        catch
        {
            // ignored
        }
    }

    private void WithdrawMoneyInBank(object sender, EventArgs e)
    {
        //Debug.Log("making a withdraw for " + (float)sender + " from an account with " + moneyInBank);
        if ( _gameModeData.moneyInBank - (float)sender >= 0)
        {
            _gameModeData.moneyInBank -= (float)sender;
            SuccessfulWithdrawal?.Invoke(true, EventArgs.Empty);

            Debug.Log("withdrawing money, money in bank now =" +  _gameModeData.moneyInBank);
        }
        else
        {
            SuccessfulWithdrawal?.Invoke(false, EventArgs.Empty);
        }
    }
}