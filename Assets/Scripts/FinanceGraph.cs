using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using System;


public class FinanceGraph : MonoBehaviour
{
    private RectTransform graphcontainer;
    [SerializeField] private Sprite circleSprite;
    private RectTransform labeltemplateX;
    private RectTransform labeltemplateY;

    private void Awake()
    {
        
        graphcontainer =transform.Find("graphcontainer").GetComponent<RectTransform>();
        labeltemplateX = graphcontainer.Find("labeltemplateX").GetComponent<RectTransform>();
        labeltemplateY = graphcontainer.Find("labeltemplateY").GetComponent<RectTransform>();
        List<int>OutputList = new List<int>() { 1,2,3,4,5};
        Queue<int> RevenueAxis = new Queue<int>(4);
        List<int>revenueList = new List<int>() { 7,12,23,34};
        Showgraph(revenueList, (float _f) => "£" + Mathf.RoundToInt(_f));
    }
    private void UpdateRevenueLine(List<int> revenueList)
    {
        int time = 0;
        
        if(time > 7)
        {
            revenueList.Clear();
            for(int i = 0; i <7 ; i++)
            {

            }
        }
    }
    private GameObject Createcircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphcontainer,false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectransform = gameObject.GetComponent<RectTransform>();
        rectransform.anchoredPosition = anchoredPosition;
        rectransform.sizeDelta = new Vector2(12, 12);
        rectransform.anchorMin = new Vector2(0, 0);
        rectransform.anchorMax = new Vector2(0, 0);
        return gameObject;

    }
    private void Showgraph(List<int> revenueList, Func<float, string> GetaxislabelY = null)
    {
       
        if (GetaxislabelY == null)
        {
            GetaxislabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }
        float graphHeight = graphcontainer.sizeDelta.y;
        float ymaximum = 0f;
        foreach (int value in revenueList)
        {
            if(value > ymaximum)
            {
                ymaximum = value;
            }
        }
        ymaximum = ymaximum * 1.2f;

        float xSize =50f;
        GameObject lastCircleGameObject = null;

        for(int i = 0; i < revenueList.Count; i++)
        {
            float xposition = i * xSize - (xSize*2);
            float yposition = (revenueList[i] / ymaximum) * graphHeight;
           GameObject circleGameObject =  Createcircle(new Vector2(xposition, yposition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }

            lastCircleGameObject = circleGameObject;
            RectTransform labelX = Instantiate(labeltemplateX);
            labelX.SetParent(graphcontainer,false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xposition, -70f);
            labelX.GetComponent<Text>().text = i.ToString();

        }
        int seperatorCount = 10;
        for(int i = 0; i<=seperatorCount; i++)
        {
            RectTransform labelY = Instantiate(labeltemplateY);
            labelY.SetParent(graphcontainer,false);
            float normalizedValue = i*1f / seperatorCount;
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-220f, normalizedValue*graphHeight);
            labelY.GetComponent<Text>().text =GetaxislabelY( normalizedValue * ymaximum);
        }
    }
    private void CreateDotConnection(Vector2 dotPositionA , Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dot connection", typeof(Image));    
        gameObject.transform.SetParent(graphcontainer, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1,.5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized; 
        float distance = Vector2.Distance(dotPositionA,dotPositionB);
        rectTransform.anchorMin =  new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance *.5f;
        rectTransform.localEulerAngles = new Vector3(0,0,UtilsClass.GetAngleFromVectorFloat(dir));

    }
}
