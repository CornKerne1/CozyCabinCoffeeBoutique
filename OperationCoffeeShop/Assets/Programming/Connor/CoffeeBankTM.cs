using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoffeeBankTM : MonoBehaviour
{
    [SerializeField] public static event EventHandler WithdrawMoney;//
    [SerializeField] public static event EventHandler SuccessfulWithdrawl;//


    public float moneyInBank;


    // Start is called before the first frame update
    void Start()
    {
        CustomerLine.DepositMoney += DepositMoneyInBank;//
        WithdrawMoney += WithdrawMoneyInBank;//

    }

    // Update is called once per frame
    void Update()
    {
    }
    void DepositMoneyInBank(object sender, EventArgs e)
    {
       
        try
        {
            moneyInBank += (float)sender;
            Debug.Log("depositing money, money in bank now =" + moneyInBank);

        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }


    }
    void WithdrawMoneyInBank(object sender, EventArgs e)
    {

        try
        {
            if (moneyInBank - (float)sender >= 0)
            {
                EventArgs ee = new EventArgs();

                SuccessfulWithdrawl?.Invoke(true, EventArgs.Empty);
                moneyInBank -= (float)sender;
                Debug.Log("withdrawing money, money in bank now =" + moneyInBank);

            }
            else
            {
                SuccessfulWithdrawl?.Invoke(false, EventArgs.Empty);

            }
        }
        catch (Exception ex)
        {
            SuccessfulWithdrawl?.Invoke(false, EventArgs.Empty);
            Debug.LogException(ex);
        }

    }
}
