using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

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
    public TextMeshProUGUI ContributionPerUnitvalue;
    public TextMeshProUGUI InventoryTurnovervalue;
    public TextMeshProUGUI Rentvalue;
    public TextMeshProUGUI CurrentLiabillitiesvalue;
    public TextMeshProUGUI ROCEvalue;
    public GameObject DbManagerGameobject;
    private List<string> UsersPortfolio = DatabaseManager.MyPortfolio;
    public Canvas CurrentCashFlows;
    public int currentmonthid;
    private int lastRecordedMonth = -1;  // Initialize to an invalid value
    private double currentCapitalAmount = 100000;
    public GameObject Timefunctionref;
    private int tdp; // used to get totaldayspassed from other script


    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {



    }
    public int GetMonthID()
    {
        Timefunctionref.gameObject.SetActive(true);
        currentmonthid = TimeFunction.CurrentMonthID;
        Timefunctionref.gameObject.SetActive(false);
        return currentmonthid;

    }
    public int GetTDP()// fetches the total no. of days passed from the timefucntion script
    {
        Timefunctionref.gameObject.SetActive(true);
        tdp = TimeFunction.totaldayspassed;
        Timefunctionref.gameObject.SetActive(false);
        return tdp;

    }
    public void CalculateRawM()// increases as the no. of products the user owns increases
    {
        Debug.Log("CalculateRawM called");
        int num = UsersPortfolio.Count;
        int rawmvalue = num * 50;
        RawMvalue.text = "£" + rawmvalue.ToString();
        CurrentCapital.text = (double.Parse(CurrentCapital.text) - rawmvalue).ToString();
    }
    public void Rent()// constant value taken from the users balance each week
    {
        Debug.Log("Rent called");
        CurrentCashFlows.gameObject.SetActive(true);
        int rent = 120;
        Rentvalue.text = "£" + rent.ToString();
        CurrentCapital.text = (double.Parse(CurrentCapital.text) - rent).ToString();
        CurrentCashFlows.gameObject.SetActive(false);
    }
    public void NCashFlow()
    {
        Debug.Log("NCashFlow called");
        int monthid = GetMonthID();
        GetNCashFlow(monthid);

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
            NCashFlowvalue.text = "£" + changeInCapital.ToString();

            // Update the current capital amount for the next iteration
            currentCapitalAmount = newAmount;
        }

        lastRecordedMonth = monthid;
    }
    public void CostOfSales()
    {

    }
    public void Weekly()
    {
        int tdp = GetTDP();
        if (tdp >= 7)
        {
            Rent();
            CalculateRawM();
            TimeFunction.totaldayspassed = 0;
        }

    }
}





