using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using System;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.VisualScripting;
using Unity.Collections.LowLevel.Unsafe;

//TRY TO MAKE IT SO RETRIEVES A Y VALUES AFTER EVERY 7 DAYS!

public class FinanceGraph : MonoBehaviour
{
    private RectTransform graphcontainer;
    [SerializeField] private Sprite circleSprite;
    private RectTransform labeltemplateX;
    private RectTransform labeltemplateY;
    private Queue<double> RevenueAxis = new Queue<double>(5);
    public Canvas FinancePage;
    public double initialval = 0;
    public static int daysforward;
    public GameObject Gameobject;



    private void Awake()
    {

        Transform foundTransform = transform.Find("graphcontainer");
        if (foundTransform != null)
        {
            Debug.Log("Graphcontainer found!");
            graphcontainer = foundTransform.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Graphcontainer not found!");
        }
        labeltemplateX = graphcontainer.Find("labeltemplateX").GetComponent<RectTransform>();
        labeltemplateY = graphcontainer.Find("labeltemplateY").GetComponent<RectTransform>();


    }
    private void Update()
    {

    }
    void Start()
    {


    }
    public void UpdateParameters()
    {
        FinancePage.gameObject.SetActive(true);
        Debug.Log("UpdateParameters method called.");
        Debug.Log("called");
        bool isQueueFull = RevenueAxis.Count == 5;// max capacity of queue
        double newyvalue = RetrieveYvalues();// gets a y value
        if (isQueueFull)
        {
            UpdateGraphData();// if queue full then dequeues and each value moves one index forward
            // deletes previous dot and dot connection
        }
        RevenueAxis.Enqueue(newyvalue);// adds the value to the end of queue
        if (newyvalue == 0)
        {
            Debug.Log("Revenue graph not updated");
        }
        Showgraph(RevenueAxis, (float _f) => "£" + Mathf.RoundToInt(_f));// displays graph
        FinancePage.gameObject.SetActive(false);
    }
    private double RetrieveYvalues()
    {
        FinancePage.gameObject.SetActive(true);
        double revenue = GetRevenue();
        Debug.Log(revenue);
        FinancePage.gameObject.SetActive(false);
        return revenue;

    }

    public double GetRevenue()
    {
        return Sales.CurrentRevenue;// gets the revenue
    }



    private GameObject Createcircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphcontainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectransform = gameObject.GetComponent<RectTransform>();
        rectransform.anchoredPosition = anchoredPosition;
        rectransform.sizeDelta = new Vector2(12, 12);
        rectransform.anchorMin = new Vector2(0, 0);
        rectransform.anchorMax = new Vector2(0, 0);
        return gameObject;

    }
    private List<Transform> yAxisLabels = new List<Transform>();
    private List<Transform> xAxisLabels = new List<Transform>();// keep track of previously created labels 
    private List<Transform> dotConnections = new List<Transform>();
    private List<GameObject> dotObjects = new List<GameObject>();

    private void Showgraph(Queue<double> revenueAxis, Func<float, string> GetAxisLabelY = null)
    {
        Debug.Log("ShowGraph method called.");
        if (revenueAxis.Count == 0)// at start of the game
        {
            Debug.LogWarning("RevenueAxis queue is empty. No data to display.");
            return;
        }
        if (GetAxisLabelY == null)
        {
            GetAxisLabelY = (float _f) => "£" + Mathf.RoundToInt(_f).ToString();
        }
        if (graphcontainer == null)
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

        float graphHeight = graphcontainer.sizeDelta.y;
        float yMaximum = (float)revenueAxis.Max();
        yMaximum = Mathf.Max(1f, yMaximum * 1.2f);// updating y maximum of the graph

        float xSize = 200f;
        GameObject lastCircleGameObject = null;

        // Instantiate Y-axis labels only once
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labeltemplateY);
            labelY.SetParent(graphcontainer, false);
            float normalizedValue = i * 1f / separatorCount;
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-220f, normalizedValue * graphHeight - 27);
            labelY.GetComponent<Text>().text = GetAxisLabelY(normalizedValue * yMaximum);
            yAxisLabels.Add(labelY);
        }

        for (int i = 0; i < revenueAxis.Count; i++)
        {
            float xPosition = i * xSize - 200f;
            float yPosition = (float)(revenueAxis.ElementAt(i) / yMaximum) * graphHeight;
            GameObject circleGameObject = Createcircle(new Vector2(xPosition, yPosition));
            dotObjects.Add(circleGameObject);  // Add the new dot to the list

            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }

            lastCircleGameObject = circleGameObject;
            RectTransform labelX = Instantiate(labeltemplateX);
            labelX.SetParent(graphcontainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -70f);
            labelX.GetComponent<Text>().text = i.ToString(); // Use i as the x-axis label
            xAxisLabels.Add(labelX);
        }
    }




    private void UpdateGraphData()
    {
        // If the queue is full, convert it to a list and update data
        // Convert the queue to a list
        List<double> revenueList = RevenueAxis.ToList();

        // Move the current items in the list one index back
        for (int i = 0; i < revenueList.Count - 1; i++)
        {
            revenueList[i] = revenueList[i + 1];
        }

        // Remove the oldest value
        revenueList.RemoveAt(revenueList.Count - 1);

        // Convert the list back to a queue
        RevenueAxis = new Queue<double>(revenueList);

    }


    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)// creates the connection between datapoints
    {
        GameObject gameObject = new GameObject("dot connection", typeof(Image));
        gameObject.transform.SetParent(graphcontainer, false);
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
}
