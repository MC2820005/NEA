using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserPortfolio : MonoBehaviour
{
    // Start is called before the first frame update
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
    public TMP_InputField StockCheck;
    private DatabaseManager databaseManager;
    
    void Start()
    {
        databaseManager = GetComponent<DatabaseManager>();

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
                ProductOne.text = productName;
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
        Debug.Log("Fetching stock for product: " + productName);

        foreach (var key in databaseManager.StockDictionary.Keys)
        {
            Debug.Log("Key in dictionary: " + key);
        }

        if (databaseManager.StockDictionary.ContainsKey(productName))
        {
            int stockQuantity = databaseManager.StockDictionary[productName];
            Debug.Log("Stock quantity for " + productName + ": " + stockQuantity);
        }
        else
        {
            Debug.Log(productName + " not found in the stock dictionary.");
            Debug.Log(string.Join(", ", databaseManager.StockDictionary.Keys));
        }


    }

}
