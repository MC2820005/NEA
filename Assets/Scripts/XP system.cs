using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.Animations;
using JetBrains.Annotations;

public class XPbar : MonoBehaviour
{
    public double CurrentXP = 0;
    public double SilverReqXP = 1000;
    public double GoldReqXP = 10000;
    public double EliteReqXP = 25000;
    public double ChampionReqXP = 70000;
    public TextMeshProUGUI RankNametext;
    public TextMeshProUGUI CurrentRanktext;
    public GameObject fillerXpBar;
    private Image fillerImage;

    void Start()
    {
        fillerXpBar = transform.Find("Filler").gameObject;
        fillerImage = fillerXpBar.GetComponent<Image>();
        if (fillerImage == null)
        {
            Debug.LogError("Filler GameObject does not have an Image component.");

        }
    }
    public void ConvertSaleToXP()
    {
        int numsales = GetNumberOfSales();
        CurrentXP += numsales * 0.8;
        Debug.Log(CurrentXP);
        Sales.totalsales = 0;

    }

    public int GetNumberOfSales()
    {
        int sales = Sales.totalsales;
        return sales;

    }
    public void RankUp()
    {
        Debug.Log("RankUp called");
        Debug.Log("CurrentXP: " + CurrentXP);




        UpdateFillerAmount(); // update xpbar for bronze rank

        // Round CurrentXP to the nearest whole number
        double roundedCurrentXP = Math.Round(CurrentXP);
        if (roundedCurrentXP >= SilverReqXP && roundedCurrentXP < GoldReqXP)
        {

            Debug.Log("RANK UP SILVER");
            RankNametext.text = "SILVER";
            // Define a silver color using RGB values
            Color silverColor = new Color(192f / 255f, 192f / 255f, 192f / 255f);
            // Set the color to silver
            RankNametext.color = silverColor;
            CurrentRanktext.color = silverColor;
            fillerImage.color = silverColor;
            if (fillerImage != null)
            {
                fillerImage.color = silverColor;
                // Reset the filler amount to 0
                fillerImage.fillAmount = 0f;
            }

            UpdateFillerAmount();
        }
        else if (roundedCurrentXP >= GoldReqXP && roundedCurrentXP < EliteReqXP)
        {
            RankNametext.text = "GOLD";
            Color goldColor = new Color(255f / 255f, 215f / 255f, 0f / 255f); // RGB values for gold
            RankNametext.color = goldColor;
            CurrentRanktext.color = goldColor;
            fillerImage.color = goldColor;
            if (fillerImage != null)
            {
                fillerImage.color = goldColor;
                // Reset the filler amount to 0
                fillerImage.fillAmount = 0f;
            }

            UpdateFillerAmount();
        }
        else if (roundedCurrentXP >= EliteReqXP && roundedCurrentXP < ChampionReqXP)
        {
            RankNametext.text = "ELITE";
            Color eliteColor = new Color(0f / 255f, 0f / 255f, 205f / 255f); // dark blue colour
            RankNametext.color = eliteColor;
            CurrentRanktext.color = eliteColor;
            fillerImage.color = eliteColor;
            if (fillerImage != null)
            {
                fillerImage.color = eliteColor;
                // Reset the filler amount to 0
                fillerImage.fillAmount = 0f;
            }

            UpdateFillerAmount();
        }
        else if (roundedCurrentXP >= ChampionReqXP)
        {
            RankNametext.text = "CHAMPION";
            Color championColor = new Color(138f / 255f, 255f / 255f, 183f / 255f); // RGB values for gold
            championColor.r += 180f / 255f;// adding some red to the gold
            championColor.r = Mathf.Clamp01(championColor.r);
            RankNametext.color = championColor;
            CurrentRanktext.color = championColor;
            fillerImage.color = championColor;
            if (fillerImage != null)
            {
                fillerImage.color = championColor;
                // Reset the filler amount to 0
                fillerImage.fillAmount = 0f;
            }

            UpdateFillerAmount();
        }
        Debug.Log("Silverrequiredxp: " + SilverReqXP);

    }
    public void UpdateFillerAmount()
    {
        // Determines the XP progress towards the next rank
        double nextRankXP = SilverReqXP; // Default to Silver
        double currentRankXP = 0;
        if (CurrentXP >= 0 && CurrentXP < SilverReqXP)
        {
            currentRankXP = 0;
            nextRankXP = SilverReqXP;

        }
        else if (CurrentXP >= SilverReqXP && CurrentXP < GoldReqXP)
        {
            nextRankXP = GoldReqXP;
            currentRankXP = SilverReqXP;
        }
        else if (CurrentXP >= GoldReqXP && CurrentXP < EliteReqXP)
        {
            nextRankXP = EliteReqXP;
            currentRankXP = GoldReqXP;
        }
        else if (CurrentXP >= EliteReqXP && CurrentXP < ChampionReqXP)
        {
            nextRankXP = ChampionReqXP;
            currentRankXP = EliteReqXP;
        }
        else
        {
            // Handle if the current XP is beyond the highest rank
            Debug.Log("Reached the highest rank");
            // Set the fill amount to 1 (full bar)
            if (fillerImage != null)
            {
                fillerImage.fillAmount = 1f;
            }
            return;
        }

        // Calculate the percentage of XP progress towards the next rank
        double progressPercentage = (CurrentXP - currentRankXP) / (nextRankXP - currentRankXP);

        // Set the fill amount of the filler bar
        if (fillerImage != null)
        {
            fillerImage.fillAmount = (float)progressPercentage; // updates the fill amount 
        }
    }





}



