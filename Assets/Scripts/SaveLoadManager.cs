using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Handles;

public class SaveLoadManager : MonoBehaviour
{
    public TimeFunction timeFunction;
    public UserPortfolio userportfolio;
    public InventoryGraph inventorygraph;
    public XPbar xpbar;
    public FinancialData financialdata;
    public void Save()
    {
        Debug.Log("called save");
        SaveData.SaveUserData(timeFunction, userportfolio, inventorygraph, xpbar, financialdata);

    }
    public void Load()
    {
        Debug.Log("called load");
        UserData data = SaveData.LoadUserData();
        if (data != null)
        {
            Debug.Log("Data is being loaded");
            timeFunction.UpdateData(data);
            userportfolio.UpdateData(data);
            inventorygraph.UpdateData(data);
            xpbar.UpdateData(data);
            financialdata.UpdateData(data);
        }
        else
        {
            Debug.LogError("Failed to load saved data.");
        }


    }
}
