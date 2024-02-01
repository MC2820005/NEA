using JetBrains.Annotations;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static UnityEditor.Handles;

public class FinancialData : MonoBehaviour
{
    // FOR EACH SUBROUTINE NEED TO CALL THEM WHEN IT HAS BEEN 7 DAYS!
    // Start is called before the first frame update
    public TextMeshProUGUI CurrentCapital;
    public TextMeshProUGUI CostOfSalesvalue;
    public TextMeshProUGUI GrossProfitvalue;
    public TextMeshProUGUI OperatingProfitvalue;
    public TextMeshProUGUI TotalCostsvalue;
    public TextMeshProUGUI Payablesvalue;
    public TextMeshProUGUI RawMvalue;
    public TextMeshProUGUI CurrentAssetsvalue;
    public TextMeshProUGUI NCashFlowvalue;
    public TextMeshProUGUI GearingRatiovalue;
    public TextMeshProUGUI InventoryTurnovervalue;
    public TextMeshProUGUI Rentvalue;
    public TextMeshProUGUI CurrentLiabillitiesvalue;
    public TextMeshProUGUI ROCEvalue;
    public TextMeshProUGUI Wagesvalue;
    public TextMeshProUGUI Recievablesvalue;
    public TextMeshProUGUI ContributionPerUnitvalue;
    public GameObject DbManagerGameobject;
    private List<string> UsersPortfolio = DatabaseManager.MyPortfolio;
    public Canvas CurrentCashFlows;
    public int currentmonthid;
    private int lastRecordedMonth = -1;  // Initialize to an invalid value
    private double currentCapitalAmount = 100000;
    public GameObject Timefunctionref;
    private int tdp; // used to get totaldayspassed from other script
    private string connectionString = "Server=localhost;Database=newproducts;User ID=root;password=Liverpool123!";
    public static double totalassets = 0;
    public static double totalstock;

    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {



    }
    public void UpdateData(UserData data)
    {
        Rentvalue.text = data.rent.ToString();
        RawMvalue.text = data.rawm.ToString();
        CostOfSalesvalue.text = data.costofsales.ToString();
        GrossProfitvalue.text = data.grossprofit.ToString();
        OperatingProfitvalue.text = data.opprofit.ToString();
        CurrentLiabillitiesvalue.text = data.cliabillities.ToString();
        CurrentAssetsvalue.text = data.cassets.ToString();
        TotalCostsvalue.text = data.totalcosts.ToString();
        Payablesvalue.text = data.payables.ToString();
        GearingRatiovalue.text = data.gearingratio.ToString();
        InventoryTurnovervalue.text = data.inventoryturnover.ToString();
        ROCEvalue.text = data.roce.ToString();
        Wagesvalue.text = data.wages.ToString();
        ContributionPerUnitvalue.text = data.cpunit.ToString();
        Recievablesvalue.text = data.recievables.ToString();

    }
    public int GetMonthID()
    {
        Timefunctionref.gameObject.SetActive(true);
        currentmonthid = TimeFunction.CurrentMonthID;
        Timefunctionref.gameObject.SetActive(false);
        return currentmonthid;// fetches id of month currently in

    }
    public int GetTDP()// fetches the total no. of days passed from the timefucntion script
    {
        Timefunctionref.gameObject.SetActive(true);
        tdp = TimeFunction.totaldayspassed;
        Timefunctionref.gameObject.SetActive(false);
        return tdp;
        // fetches a count which tells how many days have gone by
    }
    public void CalculateRawM()// increases as the no. of products the user owns increases
    {
        Debug.Log("CalculateRawM called");
        int num = UsersPortfolio.Count;// no. of products in users portfolio
        int rawmvalue = num * 50;
        RawMvalue.text = rawmvalue.ToString();
        CurrentCapital.text = (double.Parse(CurrentCapital.text) - rawmvalue).ToString();
    }// deducted from users balance
    public void Rent()// constant value taken from the users balance each week depending on their current balance
    {
        Debug.Log("Rent called");
        CurrentCashFlows.gameObject.SetActive(true);
        if (double.Parse(CurrentCapital.text) < 600000)
        {
            int rent = 120;
            Rentvalue.text = rent.ToString();
            CurrentCapital.text = Math.Round((double.Parse(CurrentCapital.text) - rent)).ToString();
        }
        else if (double.Parse(CurrentCapital.text) >= 600000 && double.Parse(CurrentCapital.text) <= 2000000)
        {
            int rent = 400;
            Rentvalue.text = rent.ToString();
            CurrentCapital.text = Math.Round((double.Parse(CurrentCapital.text) - rent)).ToString();
        }
        else
        {
            int rent = 600;
            Rentvalue.text = rent.ToString();
            CurrentCapital.text = Math.Round((double.Parse(CurrentCapital.text) - rent)).ToString();
        }
        CurrentCashFlows.gameObject.SetActive(false);// deducted from users balance

    }
    public void NCashFlow()
    {
        Debug.Log("NCashFlow called");
        int monthid = GetMonthID();// so knows what current month is
        GetNCashFlow(monthid);// value changes on a monthly basis 

    }

    public void GetNCashFlow(int monthid)
    {
        double newAmount = double.Parse(CurrentCapital.text);
        Debug.Log($"lastRecordedMonth: {lastRecordedMonth}, monthid: {monthid}");
        if (lastRecordedMonth != monthid)
        {
            Debug.Log("Next month");

            // Calculate the change in capital
            double changeInCapital = newAmount - currentCapitalAmount;
            NCashFlowvalue.text = Math.Round(changeInCapital).ToString();


            // Update the current capital amount for the next iteration
            currentCapitalAmount = newAmount;
        }

        lastRecordedMonth = monthid;
    }
    public void CostOfSales()
    {
        double cos = Sales.CurrentCostOfSales;
        double newcc = double.Parse(CurrentCapital.text) - cos;
        CurrentCapital.text = newcc.ToString();// updates user balance
        CostOfSalesvalue.text = Math.Round(cos).ToString();

    }
    public void GrossProfit()
    {
        double gp = Sales.CurrentRevenue - double.Parse(CostOfSalesvalue.text);
        GrossProfitvalue.text = Math.Round(gp).ToString();
    }
    public void OperatingProfit()
    {
        Debug.Log($"GrossProfitvalue.text: {GrossProfitvalue.text}, RawMvalue.text: {RawMvalue.text}");
        double op = double.Parse(GrossProfitvalue.text) - double.Parse(RawMvalue.text);
        OperatingProfitvalue.text = Math.Round(op).ToString();
    }
    public void Currentliabillities()
    {
        double cl = double.Parse(RawMvalue.text) + double.Parse(CostOfSalesvalue.text);
        CurrentLiabillitiesvalue.text = Math.Round(cl).ToString();

    }
    public void CurrentAssets()
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            foreach (KeyValuePair<string, int> entry in DatabaseManager.StockDictionary)
            {
                string productName = entry.Key;
                int stockAmount = entry.Value;
                try
                {
                    // Query to get the price per unit from the database
                    string query = $"SELECT PricePerUnit FROM Products WHERE ProductName = '{productName}'";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        if (result != null && double.TryParse(result.ToString(), out double pricePerUnit))
                        {
                            double productValue = pricePerUnit * stockAmount;
                            totalassets += productValue;

                            // Optionally, you can print or use the product values as needed
                            Console.WriteLine($"Product: {productName}, Stock: {stockAmount}, Price per Unit: {pricePerUnit}, Value: {productValue}");
                        }
                        else
                        {
                            // Handle the case where the price per unit is not found or not a valid double
                            Console.WriteLine($"Invalid or missing price per unit for product: {productName}");
                        }

                    }
                }
                catch (Exception er)
                {
                    Debug.Log("Error executing the query" + er.Message);

                }
            }

            // Now 'totalAssets' contains the total value of all products in stock
            Console.WriteLine($"Total Assets: {totalassets}");
            CurrentAssetsvalue.text = Math.Round(totalassets).ToString();
        }
    }
    public void TotalCosts()
    {
        Debug.Log($"Wagesvalue.text: {Wagesvalue.text}");
        double totalcosts = double.Parse(Rentvalue.text) + double.Parse(RawMvalue.text) + double.Parse(CostOfSalesvalue.text) + double.Parse(Wagesvalue.text);
        TotalCostsvalue.text = Math.Round(totalcosts).ToString();

    }
    public void Payables()
    {
        double payables = (double.Parse(Rentvalue.text) * 365) / double.Parse(CostOfSalesvalue.text);
        Payablesvalue.text = Math.Round(payables).ToString();
    }
    public void GearingRatio()
    {
        double gr = (double.Parse(Rentvalue.text) / (double.Parse(Rentvalue.text) + double.Parse(CurrentAssetsvalue.text))) * 100;
        GearingRatiovalue.text = Math.Round(gr).ToString() + "%";
    }
    public void InventoryTurnover()
    {
        foreach (KeyValuePair<string, int> entry in DatabaseManager.StockDictionary)
        {
            int stock = entry.Value;
            totalstock += stock;

        }
        double it = double.Parse(CostOfSalesvalue.text) / totalstock;
        InventoryTurnovervalue.text = Math.Round(it).ToString();
    }
    public void ROCE()
    {
        double roce = (double.Parse(OperatingProfitvalue.text) / (double.Parse(CurrentAssetsvalue.text) + double.Parse(CostOfSalesvalue.text) + double.Parse(RawMvalue.text)) * 100);
        ROCEvalue.text = Math.Round(roce).ToString() + "%";
    }
    public void CPunit()
    {
        double cpunit = Sales.CurrentRevenue / totalstock;
        ContributionPerUnitvalue.text = Math.Round(cpunit).ToString();

    }
    public void Recievables()
    {
        double rec = double.Parse(CurrentAssetsvalue.text) / Sales.CurrentRevenue;
        Recievablesvalue.text = Math.Round(rec).ToString();
    }
    public void Wages()
    {
        double wages = totalstock * 0.4;
        Wagesvalue.text = Math.Round(wages).ToString();
    }


    public void Weekly()
    {
        int tdp = GetTDP();
        if (tdp >= 7)// events occur on weekly basis
        {
            Rent();
            CalculateRawM();
            CostOfSales();
            GrossProfit();
            OperatingProfit();
            Currentliabillities();
            CurrentAssets();
            TotalCosts();
            Payables();
            GearingRatio();
            InventoryTurnover();
            ROCE();
            Wages();
            CPunit();
            Recievables();
            TimeFunction.totaldayspassed = 0;
        }

    }
}





