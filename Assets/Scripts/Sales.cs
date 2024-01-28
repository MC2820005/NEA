using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sales : MonoBehaviour
{
    private string connectionString = "Server=localhost;Database=newproducts;User ID=root;password=Liverpool123!";
    public Button OKbutton;
    public static double CurrentRevenue = 0;
    public static double CurrentCostOfSales;
    public Canvas Warning;
    public TextMeshProUGUI ProductLowStock;
    public static int totalsales = 0;




    // Your other variables
    public TextMeshProUGUI CurrentCapital;

    private void Awake()
    {


    }





    // Your other methods and code...


    void Start()
    {

    }
    void Update()
    {

    }





    public void FetchProductsData()
    {
        if (DatabaseManager.MyPortfolio != null && DatabaseManager.StockDictionary != null)
        {
            try
            {
                List<string> portfolioCopy = new List<string>(DatabaseManager.MyPortfolio);// so not modified during iteration

                foreach (string productName in portfolioCopy) // gets info about each product in users portfolio
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        string productQuery = "SELECT PricePerUnit, QualityRating, CSRating FROM Products WHERE ProductName = @productName";

                        using (MySqlCommand productCmd = new MySqlCommand(productQuery, connection))
                        {
                            productCmd.Parameters.AddWithValue("@productName", productName);

                            using (MySqlDataReader productReader = productCmd.ExecuteReader())
                            {
                                if (productReader.Read())
                                {

                                    double pricePerUnit = productReader.GetDouble("PricePerUnit");
                                    int qRating = productReader.GetInt32("QualityRating");
                                    int csRating = productReader.GetInt32("CSRating");
                                    Debug.Log("product details retrieved successully for, " + productName);
                                    productReader.Close();
                                    int stockCount = DatabaseManager.StockDictionary[productName.Trim()];// gets no. of units in stock
                                    CalculateRev(stockCount, pricePerUnit, csRating, qRating, productName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error fetching product details: " + e.Message);
            }

        }

    }

    public void CalculateRev(int stockCount, double pricePerUnit, int qRating, int csRating, string productName)
    {
        Debug.Log("CalculateRev executed. Current Revenue: " + CurrentRevenue);
        int average = (qRating + csRating) / 2;
        int checkpDay = SalesPerDay(average);
        int checkdaysGone = TimeFunction.daysforward;
        stockCount = stockCount - (checkpDay * checkdaysGone);// to see whether enough stock for this day/days
        DatabaseManager.StockDictionary[productName.Trim()] = DatabaseManager.StockDictionary[productName.Trim()] - (checkpDay * checkdaysGone);
        if (stockCount <= 0 || DatabaseManager.StockDictionary[productName.Trim()] <= 0)
        {
            Debug.Log("run out of stock");
            ProductLowStock.text = "You have run out of stock for " + productName;
            DatabaseManager.StockDictionary.Remove(productName);
            DatabaseManager.MyPortfolio.Remove(productName);
            Warning.gameObject.SetActive(true);

        }
        else
        {
            int pDay = SalesPerDay(average);
            int daysGone = TimeFunction.daysforward;// gets userinput
            double revenueAdd = daysGone * pricePerUnit * pDay;
            double costofsales = revenueAdd * 0.1;
            Debug.Log(revenueAdd);
            totalsales += pDay * daysGone;
            CurrentRevenue += revenueAdd;// updates revenue
            CurrentCostOfSales += costofsales; // updates cost of sales
            Debug.Log(CurrentRevenue);
            CurrentCapital.text = (double.Parse(CurrentCapital.text) + revenueAdd).ToString();// updates users balance

        }

    }
    public void OnClickOk()
    {
        Warning.gameObject.SetActive(false);
    }
    public int SalesPerDay(int average)// gets sales of product per day based on the products attributes e.g. quality rating
    {
        if (average >= 0 && average <= 4)
        {
            int pday = 3;
            return pday;
        }
        else if (average >= 5 && average <= 8)
        {
            int pday = 5;
            return pday;
        }
        else if (average >= 9 && average <= 12)
        {
            int pday = 7;
            return pday;
        }
        else if (average >= 13 && average <= 16)
        {
            int pday = 9;
            return pday;
        }
        else
        {
            int pday = 10;
            return pday;
        }



    }
}













