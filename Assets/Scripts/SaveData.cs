using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using static UnityEditor.Handles;

public static class SaveData
{
    
    
    public static void SaveUserData(TimeFunction timeFunction, UserPortfolio userportfolio, InventoryGraph inventorygraph, XPbar xpbar, FinancialData financialdata)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/UserData.nea";
        FileStream stream = new FileStream(path, FileMode.Create);

        UserData Data = new UserData(timeFunction, userportfolio, inventorygraph, xpbar, financialdata);

        formatter.Serialize(stream, Data);
        stream.Close();
    }

    public static UserData LoadUserData()
    {
        string path = Application.persistentDataPath + "/UserData.nea";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            UserData data = formatter.Deserialize(stream) as UserData;
            stream.Close();
            return data;

        }
        else
        {
            Debug.LogError("Save file not found in " +  path);
            return null;
        }

    }
    



}
