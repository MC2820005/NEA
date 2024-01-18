using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System;
using UnityEngine.UI;
using TMPro;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Security;

public class DatabaseManager : MonoBehaviour
{
    // Start is called before the first frame update
    private MySqlConnection connection;
    public Canvas CompensationCanvas;
    private string connectionstring = "Server=localhost;Database=newproducts;User ID=root;password=Liverpool123!"; // DATABASE DETAILS
    public TMP_InputField SearchBar;
    public Transform ProductContainer;
    public GameObject ProductPrefab;
    public TextMeshProUGUI ButtonDisplayInfo;// button created before runtime
    public bool FirstProduct; // used to assign first valid productname to the ButtonDisplayInfo
    public Canvas ProductInfoCanvas;
    public Button Purchase;
    public TextMeshProUGUI ProductNameText;
    public TextMeshProUGUI csRating;
    public TextMeshProUGUI qualityRating;
    public TextMeshProUGUI priceperunit;
    public TextMeshProUGUI suppliername;
    public TextMeshProUGUI availableunits;
    public Canvas FinancePage;
    public Canvas MyProductsPage;
    public Canvas DisplayQuantityCanvas;
    public TMP_InputField quantityInput;
    public static List<string> MyPortfolio { get; } = new List<string>();
    public static Dictionary<string, int> StockDictionary = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);


    void Start()
    {
        ConnectToDatabase();
        SearchBar.onValueChanged.AddListener(OnInputFieldValueChanged);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ConnectToDatabase()
    {
        try
        {
            connection = new MySqlConnection(connectionstring); // tries to connect to MySQL database
            connection.Open();
            Debug.Log("Connected to MySQL Database!");
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to database: " + e.Message); // if unable to connect displays error message
        }
    }
    void OnApplicationQuit()
    {
        if (connection != null && connection.State != System.Data.ConnectionState.Closed)
        {
            connection.Close();
            Debug.Log("Disconnected from the MySQL Database"); // when out of game mode back to scene the connection is closed
            MyPortfolio.Clear();
        }
    }
    public void OnInputFieldValueChanged(string input) // Checks for user input into the inputfield
    {
        if (!string.IsNullOrEmpty(input) && input.Length >= 1)// makes sure entered a character
        {
            DisplayProducts(input.Substring(0, 1));// sends this character to the subroutine
        }
        else
        {
            CreatedButtons.Clear();// if nothing entered the createdbuttons array is emptied ready for next input
            ClearProductDisplay(); // if nothing entered the options are cleared for the user
        }
    }
    public void ClearProductDisplay() // Gets rid of all the options displayed for the user
    {
        foreach (Transform child in ProductContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void DisplayProducts(string firstCharacter) // firstchar stores the char the user entered
    {
        FirstProduct = true;
        ClearProductDisplay();

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();

                string query = "SELECT DISTINCT ProductName FROM Products WHERE ProductName LIKE @firstCharacter";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@firstCharacter", firstCharacter + "%");// the condition to be met by the productnames

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool IsFirstProduct = true;  // used to allocate first product to ButtonDisplayInfo

                        while (reader.Read())
                        {
                            string productName = reader.GetString("ProductName"); // Sets product name from database to variable

                            if (IsFirstProduct) // executes only first time round to set first productname to the button
                            {
                                ButtonDisplayInfo.text = productName; // first product name is set to text component of the first button
                                IsFirstProduct = false;
                            }
                            else
                            {
                                CreateProductButton(productName); // once the text component of first button is edited a function is called to create the rest of the buttons
                            }
                        }
                    }
                }
            }
        }
        catch (Exception er)
        {
            Debug.LogError("Error executing the query: " + er.Message);
        }
    }



    private List<string> CreatedButtons = new List<string>(); // stores the name of buttons that have been created


    private void CreateProductButton(string productName)
    {


        if (!CreatedButtons.Contains(productName))
        {

            CreatedButtons.Add(productName); // add to the list

            GameObject productButton = Instantiate(ProductPrefab, ProductContainer); // create gameobject using the prefab and container

            if (productButton != null)
            {
                Button buttonComponent = productButton.GetComponent<Button>(); // access to the new button

                if (buttonComponent != null)
                {
                    TextMeshProUGUI buttonText = productButton.GetComponentInChildren<TextMeshProUGUI>(); // access to buttons text component

                    if (buttonText != null)
                    {
                        buttonText.text = productName; // sets each button to their respective productname

                        // Set the button position based on whether it's the first button or not
                        RectTransform buttonRect = productButton.GetComponent<RectTransform>(); // gets position of button

                        if (FirstProduct)
                        {
                            buttonRect.anchoredPosition = new Vector2(0, 12);
                            FirstProduct = false; // Set it to false after positioning the first button
                        }
                        else
                        {
                            RectTransform prevButtonRect = GetLastButtonRect();

                            if (prevButtonRect != null)
                            {
                                float buttonHeight = buttonRect.sizeDelta.y; // retrieves height of button
                                Debug.Log(buttonRect.sizeDelta.y);
                                Debug.Log(prevButtonRect.anchoredPosition.y);
                                buttonRect.anchoredPosition = new Vector2(prevButtonRect.anchoredPosition.x, prevButtonRect.anchoredPosition.y - buttonHeight);// positions each button
                                                                                                                                                               // directly below the next
                            }
                        }

                        buttonComponent.onClick.AddListener(() => OnProductButtonClick(productName));


                    }
                    else
                    {
                        Debug.LogError("The TextMeshProUGUI component is null.");
                    }
                }
                else
                {
                    Debug.LogError("Button component is null.");
                }
            }
            else
            {
                Debug.LogError("ProductButton is null."); // error checking methods
            }
        }
        else
        {
            Debug.Log("Button with product name " + productName + " already exists.");
        }
    }




    private RectTransform GetLastButtonRect()
    {
        if (ProductContainer.childCount > 1) // Check if there is more than one child (excluding ButtonDisplayInfo which is the product prefab)
        {
            Transform previousButton = ProductContainer.GetChild(ProductContainer.childCount - 2); // Get the last dynamically created button
            RectTransform rectTransform = previousButton.GetComponent<RectTransform>();

            if (rectTransform != null)
            {
                Debug.Log("ButtonRect Found: " + rectTransform.anchoredPosition);
                return rectTransform;
            }
            else
            {
                Debug.LogError("ButtonRect is null.");
                return null;
            }
        }
        else
        {
            Debug.LogError("No previous button found.");
            return null;
        }
    }




    public void OnProductButtonClick(string productName) // When any of the buttons are clicked apart from the first one
    {
        string QuantityInput = quantityInput.text;

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();

                // Query to fetch product details
                string productQuery = "SELECT PricePerUnit, QualityRating, CSRating, SupplierID FROM Products WHERE ProductName = @productName";// query fetching the required data

                using (MySqlCommand productCmd = new MySqlCommand(productQuery, connection))
                {
                    productCmd.Parameters.AddWithValue("@productName", productName);

                    using (MySqlDataReader productReader = productCmd.ExecuteReader())
                    {
                        if (productReader.Read())
                        {// gets respective details for the current product from the product table and assigns them to these variables
                            double PricePerUnit = productReader.GetDouble("PricePerUnit");
                            int QualityRating = productReader.GetInt32("QualityRating");
                            int CSRating = productReader.GetInt32("CSRating");
                            int SupplierID = productReader.GetInt32("SupplierID");
                            productReader.Close();// closes connection before executing a new query
                            // Fetches supplier details using the SupplierID from the products table(FOREIGN KEY)
                            string supplierQuery = "SELECT CompanyName, NumberOfAvailableUnits FROM Suppliers WHERE SupplierID = @supplierID";

                            using (MySqlCommand supplierCmd = new MySqlCommand(supplierQuery, connection))
                            {
                                supplierCmd.Parameters.AddWithValue("@supplierID", SupplierID);

                                using (MySqlDataReader supplierReader = supplierCmd.ExecuteReader())
                                {
                                    if (supplierReader.Read())
                                    {
                                        string CompanyName = supplierReader.GetString("CompanyName");
                                        int NumberOfAvailableUnits = supplierReader.GetInt32("NumberOfAvailableUnits");

                                        // Use these values as needed, for example, updating UI elements
                                        ProductInfoCanvas.gameObject.SetActive(true);
                                        ProductNameText.text = productName;
                                        csRating.text = CSRating.ToString();
                                        priceperunit.text = PricePerUnit.ToString();  // text components of TextMeshPros updated
                                        qualityRating.text = QualityRating.ToString();


                                        suppliername.text = CompanyName;
                                        availableunits.text = NumberOfAvailableUnits.ToString();
                                    }
                                }
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

    public void Return() // button on ProductInfoCanvas when clicked the ProductInfoCanvas will disappear
    {
        ProductInfoCanvas.gameObject.SetActive(false);
        CompensationCanvas.gameObject.SetActive(false);
        if (DisplayQuantityCanvas.gameObject.activeSelf)
        {
            DisplayQuantityCanvas.gameObject.SetActive(false); // when user presses ok the canvas disappears from users visibillity
        }
    }

    public void OnFirstButtonClick() // when first button displayed is clicked
    {

        string productName = ButtonDisplayInfo.text;
        Debug.Log(productName);
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionstring))
            {
                connection.Open();

                // Query to fetch product details
                string productQuery = "SELECT PricePerUnit, QualityRating, CSRating, SupplierID FROM Products WHERE ProductName = @productName";

                using (MySqlCommand productCmd = new MySqlCommand(productQuery, connection)) // fetches details for first product from MySQL database
                {
                    productCmd.Parameters.AddWithValue("@productName", productName);

                    using (MySqlDataReader productReader = productCmd.ExecuteReader())
                    {
                        if (productReader.Read())
                        {
                            double PricePerUnit = productReader.GetDouble("PricePerUnit");
                            int QualityRating = productReader.GetInt32("QualityRating");
                            int CSRating = productReader.GetInt32("CSRating");
                            int SupplierID = productReader.GetInt32("SupplierID");
                            productReader.Close();// closes connection before executing a new query
                            // Fetches supplier details using the SupplierID from the products table(FOREIGN KEY)
                            string supplierQuery = "SELECT CompanyName, NumberOfAvailableUnits FROM Suppliers WHERE SupplierID = @supplierID";

                            using (MySqlCommand supplierCmd = new MySqlCommand(supplierQuery, connection))
                            {
                                supplierCmd.Parameters.AddWithValue("@supplierID", SupplierID);

                                using (MySqlDataReader supplierReader = supplierCmd.ExecuteReader())
                                {
                                    if (supplierReader.Read())
                                    {
                                        string CompanyName = supplierReader.GetString("CompanyName");
                                        int NumberOfAvailableUnits = supplierReader.GetInt32("NumberOfAvailableUnits");

                                        // Use these values as needed, for example, updating UI elements
                                        ProductInfoCanvas.gameObject.SetActive(true);
                                        ProductNameText.text = productName;
                                        csRating.text = CSRating.ToString();
                                        priceperunit.text = PricePerUnit.ToString();
                                        qualityRating.text = QualityRating.ToString();

                                        // Additional UI elements for supplier details
                                        suppliername.text = CompanyName;
                                        availableunits.text = NumberOfAvailableUnits.ToString();
                                    }

                                }

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



    public void OnPurchaseButtonClick()
    {
        string QuantityInput = quantityInput.text; // input to inputfield

        try
        {
            if (int.TryParse(QuantityInput, out int quantity))
            {
                double currentCapital = double.Parse(FinancePage.GetComponent<Sales>().CurrentCapital.text); // gets current value of the CurrentCapital textmeshpro
                double cost = quantity * double.Parse(priceperunit.text);


                if (currentCapital >= cost && MyPortfolio.Count < 10)// must be able to afford purchase and have less than 10 products
                {
                    double remainingCapital = currentCapital - cost;
                    FinancePage.GetComponent<Sales>().CurrentCapital.text = remainingCapital.ToString(); // changes its value
                    Debug.Log("Purchase successful. Remaining balance: " + remainingCapital);
                    if (!MyPortfolio.Contains(ProductNameText.text)) // makes sure that 2 textmeshpros in MyProductsPage dont have the same productname
                    {
                        MyPortfolio.Add(ProductNameText.text);
                        int productposition = MyPortfolio.Count;
                        Debug.Log("Product purchased. Product Name: " + ProductNameText.text + ", Position: " + productposition);
                        AddToMyProductPage(productposition, ProductNameText.text);

                        Debug.Log("MyPortfolio Contents: " + string.Join(", ", MyPortfolio));
                        Debug.Log("Stock Dictionary Keys Before Update: " + string.Join(", ", StockDictionary.Keys)); // checking correct keys present in dictionary
                        UpdateStock(ProductNameText.text, quantity);
                        Debug.Log("Stock Dictionary Keys After Update: " + string.Join(", ", StockDictionary.Keys));


                    }
                    else
                    {
                        Debug.Log("Buying more stock."); // if buying more of the same product previously purchased
                        UpdateStock(ProductNameText.text, quantity);
                    }

                }
                else if (MyPortfolio.Count == 10)
                {
                    Debug.Log("Can only have 10 products in your portfolio at any 1 time.");

                }
                else
                {
                    Debug.Log("Insufficient funds. Purchase failed.");
                }
            }
            else if (string.IsNullOrWhiteSpace(QuantityInput))
            {
                Debug.Log("No input entered.");
            }
            else
            {
                Debug.Log("Invalid input");
            }

            FinancePage.gameObject.SetActive(false);
        }
        catch (FormatException e)
        {
            Debug.LogError("Error parsing input: " + e.Message);
            Debug.LogError("QuantityInput: " + QuantityInput);  // error checking methods
            Debug.LogError("PricePerUnit: " + priceperunit.text);
        }
        quantityInput.text = string.Empty;
    }
    public void UpdateStock(string productName, int quantity)
    {
        productName = productName.Trim(); // Trim white spaces

        Debug.Log("Updating stock for " + productName + ". Quantity before update: " + (StockDictionary.ContainsKey(productName) ? StockDictionary[productName].ToString() : "Not found"));
        if (StockDictionary.ContainsKey(productName))
        {
            StockDictionary[productName] += quantity; // updates value associated with the key
        }
        else
        {
            StockDictionary.Add(productName, quantity); // adds to dictionary
        }
        Debug.Log("Updated stock for " + productName + ". Quantity after update: " + StockDictionary[productName].ToString());
        Debug.Log("Stock Dictionary After Update: " + string.Join(", ", StockDictionary.Keys));
    }


    private void AddToMyProductPage(int productposition, string productName)
    {
        if (productposition >= 1 && productposition <= 10)
        {
            MyProductsPage.GetComponent<UserPortfolio>().UpdateProductText(productposition, productName);// gets component of the correct tmp based on how many different products user purchased
        }
        else
        {
            Debug.LogError("Invalid product position: " + productposition);
        }
    }





}




