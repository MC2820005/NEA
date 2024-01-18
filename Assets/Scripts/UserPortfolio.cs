using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UserPortfolio : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI CurrentCapital;
    public string productname;
    public TextMeshProUGUI Userinfo;
    public TextMeshProUGUI ProductOne;
    public TextMeshProUGUI ProductTwo;
    public TextMeshProUGUI ProductThree;
    public TextMeshProUGUI ProductFour;
    public TextMeshProUGUI ProductFive;
    public TextMeshProUGUI ProductSix;
    public TextMeshProUGUI ProductSeven;
    public TextMeshProUGUI ProductEight;
    public TextMeshProUGUI ProductNine;
    public TextMeshProUGUI ProductTen;
    public Button Xproductone;
    public Button Xproducttwo;
    public Button Xproductthree;
    public Button Xproductfour;
    public Button Xproductfive;
    public Button Xproductsix;
    public Button Xproductseven;
    public Button Xproducteight;
    public Button Xproductnine;
    public Button Xproductten;
    public TMP_InputField StockCheck;
    public Canvas DisplayQuantityCanvas;
    public TextMeshProUGUI Info;
    public Button[] productDeleteButtons;
    private string connectionstring = "Server=localhost;Database=newproducts;User ID=root;password=Liverpool123!"; // DATABASE DETAILS
    public Canvas CompensationCanvas;
    public static double compensation;




    void Start()
    {
        productDeleteButtons = new Button[]
        {
        Xproductone,
        Xproducttwo,
        Xproductthree,
        Xproductfour,
        Xproductfive,
        Xproductsix,
        Xproductseven,
        Xproducteight,
        Xproductnine,
        Xproductten
        };

        foreach (Button button in productDeleteButtons)
        {
            button.onClick.AddListener(() => RemoveProduct(button.name));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateProductText(int productposition, string productName)
    {

        Debug.Log("Updating product at position: " + productposition + " with name: " + productName);
        switch (productposition)
        {
            case 1:
                ProductOne.text = productName; // makes sure that productname gets placed at correct position based on whether the users first/second purchase
                break;
            case 2:
                ProductTwo.text = productName;
                break;
            case 3:
                ProductThree.text = productName;
                break;
            case 4:
                ProductFour.text = productName;
                break;
            case 5:
                ProductFive.text = productName;
                break;
            case 6:
                ProductSix.text = productName;
                break;
            case 7:
                ProductSeven.text = productName;
                break;
            case 8:
                ProductEight.text = productName;
                break;
            case 9:
                ProductNine.text = productName;
                break;
            case 10:
                ProductTen.text = productName;
                break;
            default:
                Debug.LogError("Invalid product index: " + productposition);
                break;
        }
    }
    public void FetchStockNum()
    {
        string productName = StockCheck.text.Trim();
        Debug.Log("Trimmed product name: " + productName);
        Debug.Log("Fetching stock for product: " + productName);

        // Debug statements to check StockDictionary keys
        Debug.Log("Stock Dictionary Keys Before Check: " + string.Join(", ", DatabaseManager.StockDictionary.Keys));


        if (DatabaseManager.StockDictionary.ContainsKey(productName))
        {
            int stockQuantity = DatabaseManager.StockDictionary[productName];
            Debug.Log("Stock quantity for " + productName + ": " + stockQuantity);
            DisplayQuantityCanvas.gameObject.SetActive(true);
            Info.text = "Stock quantity for " + productName + " = " + stockQuantity; // displayed for user
        }
        else
        {
            Debug.Log(productName + " not found in the stock dictionary."); // when user types invalid productname
            Debug.Log("Stock Dictionary Keys After Check: " + string.Join(", ", DatabaseManager.StockDictionary.Keys));
        }
    }
    public void RemoveProduct(string rbuttonname)
    {
        int productposition;

        if (rbuttonname == "Xproduct1")
        {
            productposition = 1;

        }
        else if (rbuttonname == "Xproduct2")
        {
            productposition = 2;

        }
        else if (rbuttonname == "Xproduct3")
        {
            productposition = 3;

        }
        else if (rbuttonname == "Xproduct4")
        {
            productposition = 4;

        }
        else if (rbuttonname == "Xproduct5")
        {
            productposition = 5;

        }
        else if (rbuttonname == "Xproduct6")
        {
            productposition = 6;

        }
        else if (rbuttonname == "Xproduct7")
        {
            productposition = 7;

        }
        else if (rbuttonname == "Xproduct8")
        {
            productposition = 8;

        }
        else if (rbuttonname == "Xproduct9")
        {
            productposition = 9;

        }
        else if (rbuttonname == "Xproduct10")
        {
            productposition = 10;

        }
        else
        {
            productposition = 0; // so doesnt remove text component of any productname if user doesnt click on any of the buttons
        }


        switch (productposition)
        {
            case 1:
                string productnameone = ProductOne.text;
                ProductOne.text = string.Empty;
                FetchProductDetails(productnameone);
                RemoveFromDict(productnameone);
                if (productnameone != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnameone))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnameone);
                        Debug.Log("Product removed from MyPortfolio: " + productnameone);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnameone);
                    }

                }

                if (productnameone != string.Empty)
                {

                }


                break;
            case 2:
                string productnametwo = ProductTwo.text;
                ProductTwo.text = string.Empty;
                FetchProductDetails(productnametwo);
                RemoveFromDict(productnametwo);
                if (productnametwo != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnametwo))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnametwo);
                        Debug.Log("Product removed from MyPortfolio: " + productnametwo);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnametwo);
                    }

                }


                break;
            case 3:
                string productnamethree = ProductThree.text;
                ProductThree.text = string.Empty;
                FetchProductDetails(productnamethree);
                RemoveFromDict(productnamethree);
                if (productnamethree != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnamethree))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnamethree);
                        Debug.Log("Product removed from MyPortfolio: " + productnamethree);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnamethree);
                    }
                }

                break;
            case 4:
                string productnamefour = ProductFour.text;
                ProductFour.text = string.Empty;
                FetchProductDetails(productnamefour);
                RemoveFromDict(productnamefour);
                if (productnamefour != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnamefour))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnamefour);
                        Debug.Log("Product removed from MyPortfolio: " + productnamefour);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnamefour);
                    }
                }

                break;
            case 5:
                string productnamefive = ProductFive.text;
                ProductFive.text = string.Empty;
                FetchProductDetails(productnamefive);
                RemoveFromDict(productnamefive);
                if (productnamefive != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnamefive))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnamefive);
                        Debug.Log("Product removed from MyPortfolio: " + productnamefive);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnamefive);
                    }

                }


                break;
            case 6:
                string productnamesix = ProductSix.text;
                ProductSix.text = string.Empty;
                FetchProductDetails(productnamesix);
                RemoveFromDict(productnamesix);
                if (productnamesix != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnamesix))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnamesix);
                        Debug.Log("Product removed from MyPortfolio: " + productnamesix);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnamesix);
                    }
                }


                break;
            case 7:
                string productnameseven = ProductSeven.text;
                ProductSeven.text = string.Empty;
                FetchProductDetails(productnameseven);
                RemoveFromDict(productnameseven);
                if (productnameseven != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnameseven))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnameseven);
                        Debug.Log("Product removed from MyPortfolio: " + productnameseven);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnameseven);
                    }

                }

                break;
            case 8:
                string productnameeight = ProductEight.text;
                ProductEight.text = string.Empty;
                FetchProductDetails(productnameeight);
                RemoveFromDict(productnameeight);
                if (productnameeight != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnameeight))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnameeight);
                        Debug.Log("Product removed from MyPortfolio: " + productnameeight);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnameeight);
                    }

                }

                break;
            case 9:
                string productnamenine = ProductNine.text;
                ProductNine.text = string.Empty;
                FetchProductDetails(productnamenine);
                RemoveFromDict(productnamenine);
                if (productnamenine != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnamenine))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnamenine);
                        Debug.Log("Product removed from MyPortfolio: " + productnamenine);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnamenine);
                    }

                }


                break;
            case 10:
                string productnameten = ProductTen.text;
                ProductTen.text = string.Empty;
                FetchProductDetails(productnameten);
                RemoveFromDict(productnameten);
                if (productnameten != string.Empty)
                {
                    if (DatabaseManager.MyPortfolio.Contains(productnameten))
                    {
                        DatabaseManager.MyPortfolio.Remove(productnameten);
                        Debug.Log("Product removed from MyPortfolio: " + productnameten);
                    }
                    else
                    {
                        Debug.Log("Product not found in MyPortfolio: " + productnameten);
                    }

                }
                break;
            default:
                Debug.LogError("Invalid product index: " + productposition);
                break;
        }
    }

    private void FetchProductDetails(string productName)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();

                string productQuery = "SELECT PricePerUnit FROM Products WHERE ProductName = @productName";

                using (MySqlCommand productCmd = new MySqlCommand(productQuery, connection))
                {
                    productCmd.Parameters.AddWithValue("@productName", productName);

                    using (MySqlDataReader productReader = productCmd.ExecuteReader())
                    {
                        if (productReader.Read())
                        {
                            double PricePerUnit = productReader.GetDouble("PricePerUnit");
                            productReader.Close();

                            int stockcount = DatabaseManager.StockDictionary[productName];
                            double returns = stockcount * PricePerUnit / 2;
                            CurrentCapital.text = Convert.ToString(double.Parse(CurrentCapital.text) + returns);

                            CompensationCanvas.gameObject.SetActive(true);
                            Userinfo.text = "You have been compensated £ " + returns;
                            compensation = returns;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error fetching product details for " + productName + ": " + e.Message);
        }
    }
    public void RemoveFromDict(string productname)
    {
        if (!string.IsNullOrEmpty(productname))
        {
            if (DatabaseManager.StockDictionary.ContainsKey(productname))
            {
                DatabaseManager.StockDictionary.Remove(productname);
                Debug.Log("Product removed from StockDictionary: " + productname);
            }
            else
            {
                Debug.Log("Product not found in StockDictionary: " + productname);
            }
        }

    }



}
