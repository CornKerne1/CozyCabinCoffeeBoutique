using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResearchDictionary : MonoBehaviour
{
    private Dictionary<string, int> ResearchDict = new Dictionary<string, int>();

    public Text researchDisplay;

    private void Start()
    {

        DisplayResearchList();
    }
    public void DisplayResearchList()
    {
        researchDisplay.text = "";

        if (ResearchDict.Count <= 0)
        {
            researchDisplay.text = "Need more Research.";
        }
        else
        {

            foreach (var item in ResearchDict)
            {
                researchDisplay.text += "item: " + item.Key + " - Quantity: " + item.Value + "\n";
            }
        }
    }
    public void AddBeans()
    {
        if (ResearchDict.ContainsKey("Beans"))
        {
            ResearchDict["Beans"]++;

        }
        else
        {
            ResearchDict.Add("Beans", 1);
        }
        DisplayResearchList();
    }
    public void RemoveBeans()
    {
        if (ResearchDict.ContainsKey("Beans"))
        {
            ResearchDict["Beans"]--;

        }
        if (ResearchDict["Beans"] <= 0)
        {
            ResearchDict.Remove("Beans");
        }

        DisplayResearchList();
    }
    public void AddMilk()
    {
        if (ResearchDict.ContainsKey("Milk"))
        {
            ResearchDict["Milk"]++;

        }
        else
        {
            ResearchDict.Add("Milk", 1);
        }
        DisplayResearchList();
    }
    public void RemoveMilk()
    {
        if (ResearchDict.ContainsKey("Milk"))
        {
            ResearchDict["Milk"]--;

        }
        if (ResearchDict["Milk"] <= 0)
        {
            ResearchDict.Remove("Milk");
        }

        DisplayResearchList();
    }
}
