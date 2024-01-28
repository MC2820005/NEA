using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;
using TMPro;



public class InventoryGraph : MonoBehaviour
{
    private List<Transform> yAxisLabels = new List<Transform>();
    private List<Transform> xAxisLabels = new List<Transform>();// keep track of previously created labels 
    private List<Transform> dotConnections = new List<Transform>();
    private List<GameObject> dotObjects = new List<GameObject>();
    private RectTransform Graphcontainer;
    [SerializeField] private Sprite circleSprite;
    private RectTransform labeltemplateX;
    private RectTransform labeltemplateY;
    private Queue<double> StockAxis = new Queue<double>(5);
    public Canvas InventoryPage;
    public double totalstock;
    public TMP_InputField BufferStock;
    public TMP_InputField ReOrderStock;
    public Canvas WarningBuffer;
    public int BufferStockval;
    public int ReorderStockval;
    public Canvas ReoderLevelRemind;

    void Start()
    {
        
    }
    private void Awake()
    {

        Transform foundTransform = transform.Find("Graphcontainer");
        if (foundTransform != null)
        {
            Debug.Log("Graphcontainer found!");
            Graphcontainer = foundTransform.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Graphcontainer not found!");
        }
        labeltemplateX = Graphcontainer.Find("LabelTemplateXaxis").GetComponent<RectTransform>();
        labeltemplateY = Graphcontainer.Find("LabelTemplateYaxis").GetComponent<RectTransform>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BufferStockLevel(string bufferinput)
    {
        Debug.Log("BufferStockLevel called");
        bufferinput = BufferStock.text;
        int.TryParse(bufferinput ,out BufferStockval);
        Debug.Log(BufferStockval);
        Debug.Log(totalstock);
        if(!int.TryParse(bufferinput , out BufferStockval))
        {
            Debug.Log("Invalid Input");
        }
        else
        {
            if(totalstock <= BufferStockval)
            {
                WarningBuffer.gameObject.SetActive(true);
            }
        }
        

    }
    public void ReOrderLevel(string reorderinput)
    {
        Debug.Log("ReOrderLevel called");
        reorderinput = ReOrderStock.text;
        int.TryParse(reorderinput, out ReorderStockval);
        if (!int.TryParse(reorderinput, out BufferStockval))
        {
          Debug.Log("Invalid Input");
        }
        else
        {
            if (totalstock <= ReorderStockval)
            {
                ReoderLevelRemind.gameObject.SetActive(true);
               
            }
        }

    }
    public void ReturnToPrev()
    {
        if(WarningBuffer.gameObject.activeSelf)
        {
            WarningBuffer.gameObject.SetActive(false);
        }
        if(ReoderLevelRemind.gameObject.activeSelf)
        {
            ReoderLevelRemind.gameObject.SetActive(false);    
        }
    }
    public void UpdateGraphParameters()
    {
        InventoryPage.gameObject.SetActive(true);
        Debug.Log("UpdateGraphParameters method called.");
        Debug.Log("called");
        bool isQueueFull = StockAxis.Count == 5;// max capacity of queue
        double newyvalue = GetYvalue();//gets a y value
        Debug.Log(newyvalue);
        if (isQueueFull)
        {
            UpdateGraph();// if queue full then dequeues and each value moves one index forward
            // deletes previous dot and dot connection
        }
        StockAxis.Enqueue(newyvalue);// adds the value to the end of queue
        if (newyvalue == 0)
        {
            Debug.Log("Stock graph not updated");
        }
        Showgraph(StockAxis, (float _f) => Mathf.RoundToInt(_f).ToString());// displays graph
        InventoryPage.gameObject.SetActive(false);
    }
    private double GetYvalue()
    {
        double newyval;
        totalstock = 0; // Reset totalstock to zero before summing up the stock values
        foreach (var kvp in DatabaseManager.StockDictionary)
        {
            int units = kvp.Value; // Get the units for the current product name
            totalstock += units;   // Add the units to the total stock
        }
        newyval = totalstock;
        return newyval;
    }

    private void UpdateGraph()
    {
        // If the queue is full, convert it to a list and update data
        // Convert the queue to a list
        List<double> StockList = StockAxis.ToList();

        // Move the current items in the list one index back
        for (int i = 0; i < StockList.Count - 1; i++)
        {
            StockList[i] = StockList[i + 1];
        }

        // Remove the oldest value
        StockList.RemoveAt(StockList.Count - 1);

        // Convert the list back to a queue
        StockAxis = new Queue<double>(StockList);

    }

    private GameObject Createcircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(Graphcontainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectransform = gameObject.GetComponent<RectTransform>();
        rectransform.anchoredPosition = anchoredPosition;
        rectransform.sizeDelta = new Vector2(12, 12);
        rectransform.anchorMin = new Vector2(0, 0);
        rectransform.anchorMax = new Vector2(0, 0);
        return gameObject;

    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)// creates the connection between datapoints
    {
        GameObject gameObject = new GameObject("dot connection", typeof(Image));
        gameObject.transform.SetParent(Graphcontainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));

        // Add the new dot connection to the list
        dotConnections.Add(rectTransform);
    }
    private void Showgraph(Queue<double> StockAxis, Func<float, string> GetAxisLabelY = null)
    {
        Debug.Log("ShowGraph method called.");
        if (StockAxis.Count == 0)// at start of the game
        {
            Debug.LogWarning("RevenueAxis queue is empty. No data to display.");
            return;
        }
        if (GetAxisLabelY == null)
        {
            GetAxisLabelY = (float _f) => "£" + Mathf.RoundToInt(_f).ToString();
        }
        if (Graphcontainer == null)
        {
            Debug.LogError("GraphContainer is null!");
            return;
        }

        // Destroy previous Y-axis labels, X-axis labels, dots, and dot connections
        foreach (Transform label in yAxisLabels)
        {
            Destroy(label.gameObject);
        }
        yAxisLabels.Clear();

        foreach (Transform label in xAxisLabels)
        {
            Destroy(label.gameObject);
        }
        xAxisLabels.Clear();

        foreach (GameObject dot in dotObjects)
        {
            Destroy(dot.gameObject);
        }
        dotObjects.Clear();

        foreach (Transform connection in dotConnections)
        {
            Destroy(connection.gameObject);
        }
        dotConnections.Clear();

        float graphHeight = Graphcontainer.sizeDelta.y;
        float yMaximum = (float)StockAxis.Max();
        yMaximum = Mathf.Max(1f, yMaximum * 1.2f);// updating y maximum of the graph

        float xSize = 200f;
        GameObject lastCircleGameObject = null;

        // Instantiate Y-axis labels only once
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labeltemplateY);
            labelY.SetParent(Graphcontainer, false);
            float normalizedValue = i * 1f / separatorCount;
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-490f, 4 * normalizedValue * graphHeight - 109);
            Debug.Log(4 * normalizedValue * graphHeight - 109);
            labelY.GetComponent<Text>().text = GetAxisLabelY(normalizedValue * yMaximum);
            yAxisLabels.Add(labelY);
        }

        for (int i = 0; i < StockAxis.Count; i++)
        {
            float xPosition = -469f + i * xSize * 0.9f;
            float yPosition = (float)(StockAxis.ElementAt(i) / yMaximum) * graphHeight;
            GameObject circleGameObject = Createcircle(new Vector2(xPosition, yPosition));
            dotObjects.Add(circleGameObject);  // Add the new dot to the list

            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }

            lastCircleGameObject = circleGameObject;
            RectTransform labelX = Instantiate(labeltemplateX);
            labelX.SetParent(Graphcontainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -130f);
            labelX.GetComponent<Text>().text = i.ToString(); // Use i as the x-axis label
            xAxisLabels.Add(labelX);
        }
    }
}
