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
        Debug.Log("depositing money");
        try
        {
            moneyInBank += (float)sender;

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
