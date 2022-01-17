using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ShoppingDict : MonoBehaviour
{
    private Dictionary<string, int> ShoppingDic = new Dictionary<string, int>();

    public Text shoppingDisplay;

    private void Start()
    {

        DisplayShoppingList();
    }
    public void DisplayShoppingList()
    {
        shoppingDisplay.text = "";

        if (ShoppingDic.Count <= 0)
        {
            shoppingDisplay.text = "Shopping List Is Empty.";
        }
        else
        {

            foreach (var item in ShoppingDic)
            {
                shoppingDisplay.text += "item: " + item.Key + " - Quantity: " + item.Value + "\n";
            }
        }
    }
    public void AddBeans()
    {
        if (ShoppingDic.ContainsKey("Beans")){
            ShoppingDic["Beans"]++;

        }
        else
        {
            ShoppingDic.Add("Beans", 1);
        }
        DisplayShoppingList();
    }
    public void RemoveBeans()
    {
        if (ShoppingDic.ContainsKey("Beans"))
        {
            ShoppingDic["Beans"]--;

        }
        if (ShoppingDic["Beans"] <= 0)
        {
            ShoppingDic.Remove("Beans");
        }

        DisplayShoppingList();
    }

}
