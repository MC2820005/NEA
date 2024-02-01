using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Handles;

[Serializable]
public class UserData
{
    // Start is called before the first frame update
    public double Currentcapital;
    public double totalstock;
    public double Revenue;
    public List<string> TheUserProducts;
    public Dictionary<string,int> UsersStock;
    public int DAYofMONTH;
    public string DAYofWEEK;
    public int YEAR;
    public string MonthName;
    public string CurrentRank;
    public double CurrentXP;
    public double rent;
    public double rawm;
    public double costofsales;
    public double grossprofit;
    public double opprofit;
    public double cliabillities;
    public double cassets;
    public double totalcosts;
    public double payables;
    public double gearingratio;
    public double inventoryturnover;
    public double roce;
    public double wages;
    public double cpunit;
    public double recievables;
    public int BufferStock;
    public int ReOrderLevel;
    public string firstproduct;
    public string secondproduct;
    public string thirdproduct;
    public string fourthproduct;
    public string fifthproduct;
    public string sixthproduct;
    public string seventhproduct;
    public string eigthproduct;
    public string ninthproduct;
    public string tenthproduct;
    public float fillerxpbar;
    



    public UserData(TimeFunction timeFunction, UserPortfolio userportfolio, InventoryGraph inventorygraph, XPbar xpbar, FinancialData financialdata)
    {
        Currentcapital = double.Parse(userportfolio.CurrentCapital.text);
        totalstock = inventorygraph.totalstock;
        Revenue = Sales.CurrentRevenue;
        TheUserProducts = DatabaseManager.MyPortfolio;
        UsersStock = DatabaseManager.StockDictionary;
        DAYofMONTH = timeFunction.CurrentDayofMonth;
        MonthName = timeFunction.CurrentMonthName;
        DAYofWEEK = timeFunction.CurrentWeekDay;
        YEAR = timeFunction.CurrentYear;
        CurrentRank = xpbar.RankNametext.text;
        CurrentXP = xpbar.CurrentXP;
        firstproduct = userportfolio.ProductOne.text;
        secondproduct = userportfolio.ProductTwo.text;
        thirdproduct = userportfolio.ProductThree.text;
        fourthproduct = userportfolio.ProductFour.text;
        fifthproduct = userportfolio.ProductFive.text;
        sixthproduct = userportfolio.ProductSix.text;
        seventhproduct = userportfolio.ProductSeven.text;
        eigthproduct = userportfolio.ProductEight.text;
        ninthproduct = userportfolio.ProductNine.text;
        tenthproduct = userportfolio.ProductTen.text;
        rent = double.Parse(financialdata.Rentvalue.text);
        rawm = double.Parse(financialdata.RawMvalue.text);
        costofsales = double.Parse(financialdata.CostOfSalesvalue.text);
        grossprofit = double.Parse(financialdata.GrossProfitvalue.text);
        opprofit = double.Parse(financialdata.OperatingProfitvalue.text);
        cliabillities = double.Parse(financialdata.CurrentLiabillitiesvalue.text);
        cassets = double.Parse(financialdata.CurrentAssetsvalue.text);
        totalcosts = double.Parse(financialdata.TotalCostsvalue.text);
        payables = double.Parse(financialdata.Payablesvalue.text);
        if (double.TryParse(financialdata.GearingRatiovalue.text, out gearingratio))
        {
            Debug.Log("Converted Gratio");
        }
        else
        {
            Debug.Log("Unable to convert Gratio ");
        }
        inventoryturnover = double.Parse(financialdata.InventoryTurnovervalue.text);
        if (double.TryParse(financialdata.ROCEvalue.text, out roce))
        {
            Debug.Log("Converted roce");
        }
        else
        {
            Debug.Log("Unable to convert roce ");
        }
        wages = double.Parse(financialdata.Wagesvalue.text);
        cpunit = double.Parse(financialdata.ContributionPerUnitvalue.text);
        recievables = double.Parse(financialdata.Recievablesvalue.text);
        ReOrderLevel = inventorygraph.ReorderStockval;
        BufferStock = inventorygraph.BufferStockval;


       


    }

}
