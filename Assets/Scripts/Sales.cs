using MySql.Data.MySqlClient;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sales : MonoBehaviour
{
    private string connectionString = "Server=localhost;Database=newproducts;User ID=root;password=Liverpool123!";
    public Button OKbutton;
    public static double CurrentRevenue = 0;





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
        try
        {
            foreach (string productName in DatabaseManager.MyPortfolio)
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

                                productReader.Close();
                                int stockCount = DatabaseManager.StockDictionary[productName.Trim()];
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

    public void CalculateRev(int stockCount, double pricePerUnit, int qRating, int csRating, string productName)
    {
        Debug.Log("CalculateRev executed. Current Revenue: " + CurrentRevenue);
        int average = (qRating + csRating) / 2;
        int checkpDay = SalesPerDay(average);
        int checkdaysGone = TimeFunction.daysforward;
        stockCount = stockCount - (checkpDay * checkdaysGone);// to see whether enough stock for this day/days
        if (stockCount <= 0)
        {
            Debug.Log("You have run out of stock for " + productName);
        }
        else
        {
            int pDay = SalesPerDay(average);
            int daysGone = TimeFunction.daysforward;
            double revenueAdd = daysGone * pricePerUnit * pDay;
            Debug.Log(revenueAdd);
            CurrentRevenue += revenueAdd;
            Debug.Log(CurrentRevenue);
            CurrentCapital.text = (double.Parse(CurrentCapital.text) + revenueAdd).ToString();

        }

    }
    public int SalesPerDay(int average)
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













