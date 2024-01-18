using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeFunction : MonoBehaviour
{
    public TMP_Text Year;
    public TMP_Text DayOfMonth;
    public TMP_Text Weekday;
    public TMP_InputField Input;
    public int CurrentYear = 2023;
    public int CurrentMonthID = 01; // identifies which month it currently is
    public string CurrentMonthName;
    public int CurrentDayofMonth = 1;
    public string CurrentWeekDay;
    public int WeekDayIndex = 2; // changes value depending on user input 
    public static int daysforward;
    public RectTransform Graphcontainer;
    public Canvas FinancePage;


    void Start()
    {

    }
    void Update()
    {



    }


    public void MoveForward(string timefunctioninput)
    {

        Year.rectTransform.anchoredPosition = new Vector2(-16, 200); // makes sure that when the values of these change their postion remains a constant so they dont move 
        DayOfMonth.rectTransform.anchoredPosition = new Vector2(-16, 133);
        Weekday.rectTransform.anchoredPosition = new Vector2(-16, 63);

        timefunctioninput = Input.text;// takes the input from user
        int.TryParse(timefunctioninput, out int value); // attempts to convert it to int
        Debug.Log(value);
        if (!int.TryParse(timefunctioninput, out value))
        {
            Debug.Log("invalid input");
        }
        else if (value > 7)
        {
            Debug.Log("Cannot go forward by an interval greater than 1 week.");
        }
        if (value <= 7)
        {
            CurrentDayofMonth = CurrentDayofMonth + value;// day of month updated when user inputs an integer value within a specific range (1-7)
            daysforward = value;


        }
        MonthsYears(value);
        string[] WeekDay = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

        if (value <= 7)
        {
            WeekDayIndex = WeekDayIndex + value; //day of week updated when user inputs an integer value within the range
            ValidWeekday(WeekDay, WeekDayIndex);

        }
        else
        {
            CurrentWeekDay = WeekDay[WeekDayIndex];
        }
        Year.text = CurrentYear.ToString(); // link text mesh pros to these variables which will change during runtime
        DayOfMonth.text = CurrentMonthName + " " + CurrentDayofMonth.ToString();
        Weekday.text = CurrentWeekDay;
        Input.text = string.Empty;


    }

    public void MonthsYears(int value) // sets no. of days in each month and makes sure that the Month Name
                                       // and the day of the months update when the user has reached the end of a particular month
    {
        int JanDays = 31;
        int FebDays = 28;
        int MarDays = 31;
        int AprDays = 30;
        int MayDays = 31;
        int JunDays = 30;
        int JulDays = 31;
        int AugDays = 31;
        int SepDays = 30;
        int OctDays = 31;
        int NovDays = 30;
        int DecDays = 31;
        int diff;
        int startval;
        if (CurrentMonthID == 01)
        {
            CurrentMonthName = "January";

            if (CurrentDayofMonth > JanDays)
            {
                CurrentMonthID = 02;
                diff = CurrentDayofMonth - JanDays;
                startval = diff;
                CurrentDayofMonth = startval;// when going to next month it begins on the correct Dayofthemonth depending on the user input
            }
        }
        if (CurrentMonthID == 02)
        {
            CurrentMonthName = "February";

            if (CurrentDayofMonth > FebDays)
            {
                CurrentMonthID = 03;
                diff = CurrentDayofMonth - FebDays;
                startval = diff;
                CurrentDayofMonth = startval;


            }
        }
        if (CurrentMonthID == 03)
        {
            CurrentMonthName = "March";

            if (CurrentDayofMonth > MarDays)
            {
                CurrentMonthID = 04;
                diff = CurrentDayofMonth - MarDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 04)
        {
            CurrentMonthName = "April";

            if (CurrentDayofMonth > AprDays)
            {
                CurrentMonthID = 05;
                diff = CurrentDayofMonth - AprDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 05)
        {
            CurrentMonthName = "May";

            if (CurrentDayofMonth > MayDays)
            {
                CurrentMonthID = 06;
                diff = CurrentDayofMonth - MayDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 06)
        {
            CurrentMonthName = "June";
            if (CurrentDayofMonth > JunDays)
            {
                CurrentMonthID = 07;
                diff = CurrentDayofMonth - JunDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 07)
        {
            CurrentMonthName = "July";
            if (CurrentDayofMonth > JulDays)
            {
                CurrentMonthID = 08;
                diff = CurrentDayofMonth - JulDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 08)
        {
            CurrentMonthName = "August";
            if (CurrentDayofMonth > AugDays)
            {
                CurrentMonthID = 09;
                diff = CurrentDayofMonth - AugDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 09)
        {
            CurrentMonthName = "September";
            if (CurrentDayofMonth > SepDays)
            {
                CurrentMonthID = 10;
                diff = CurrentDayofMonth - SepDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 10)
        {
            CurrentMonthName = "October";
            if (CurrentDayofMonth > OctDays)
            {
                CurrentMonthID = 11;
                diff = CurrentDayofMonth - OctDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 11)
        {
            CurrentMonthName = "November";
            if (CurrentDayofMonth > NovDays)
            {
                CurrentMonthID = 12;
                diff = CurrentDayofMonth - NovDays;
                startval = diff;
                CurrentDayofMonth = startval;

            }
        }
        if (CurrentMonthID == 12)
        {
            CurrentMonthName = "December";
            if (CurrentDayofMonth > DecDays)
            {
                CurrentMonthID = 01;
                diff = CurrentDayofMonth - DecDays;
                startval = diff;
                CurrentDayofMonth = startval;
                CurrentYear = CurrentYear += 1; // 
            }
        }

    }
    public void ValidWeekday(string[] WeekDay, int WeekDayIndex)// makes sure CurrentWeekDay is always assigned a value in the array                       
    {
        if (WeekDayIndex >= 7)//when day of week goes past sunday the CurrentWeekDay should go to the corresponding day in the next week depending on the user input
        {
            WeekDayIndex = WeekDayIndex % 7;
            CurrentWeekDay = WeekDay[WeekDayIndex];
        }
        else
        {
            CurrentWeekDay = WeekDay[WeekDayIndex];
        }


    }

}
