using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    void Start()
    {

    }

    // Update is called once per frame

    void Update()
    {



    }
    public void CalculateRawM()// increases as the no. of products the user owns increases
    {
        Debug.Log("CalculateRawM called");
        int num = UsersPortfolio.Count;
        int rawmvalue = num * 50;
        RawMvalue.text = rawmvalue.ToString();
        CurrentCapital.text = (double.Parse(CurrentCapital.text) - rawmvalue).ToString();
    }
    public void Rent()// constant value taken from the users balance each week
    {
        Debug.Log("Rent called");
        CurrentCashFlows.gameObject.SetActive(true);
        int rent = 120;
        Rentvalue.text = rent.ToString();
        CurrentCapital.text = (double.Parse(CurrentCapital.text) - rent).ToString();
        CurrentCashFlows.gameObject.SetActive(false);
    }
    public void OnTFunctionButtonClick()
    {

    }


}
