using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class XPbar : MonoBehaviour
{
    public int salesvolume;
    public double currentxp =0;
    public double Silverrequiredxp = 5000;
    public double Goldrequiredxp = 12000;
    public double Eliterequiredxp = 25000;
    public double Championrequiredxp = 50000;

    public void ConvertSaleToXP(int salesvolume)
    {
        double xpgained = salesvolume * 0.1;
        currentxp = currentxp =+ xpgained;
    }
    public void RankUp(double currentxp)
    {
        if (currentxp < Silverrequiredxp)
        {

        }
        
            

    }


}
