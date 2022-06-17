using UnityEngine;
using System;

public class CoffeeBankTM : MonoBehaviour
{
    public static event EventHandler SuccessfulWithdrawal;


    public float moneyInBank = 30;


    private void Start()
    {
        CustomerLine.DepositMoney += DepositMoneyInBank;
        ComputerShop.SpendMoney += WithdrawMoneyInBank;
    }

    private void DepositMoneyInBank(object sender, EventArgs e)
    {
        try
        {
            moneyInBank += (float)sender;
        }
        catch
        {
            // ignored
        }
    }

    private void WithdrawMoneyInBank(object sender, EventArgs e)
    {
        //Debug.Log("making a withdraw for " + (float)sender + " from an account with " + moneyInBank);
        if (moneyInBank - (float)sender >= 0)
        {
            moneyInBank -= (float)sender;
            SuccessfulWithdrawal?.Invoke(true, EventArgs.Empty);

            Debug.Log("withdrawing money, money in bank now =" + moneyInBank);
        }
        else
        {
            SuccessfulWithdrawal?.Invoke(false, EventArgs.Empty);
        }
    }
}