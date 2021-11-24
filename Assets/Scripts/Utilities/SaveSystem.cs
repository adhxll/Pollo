using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    //CALL THIS FUNCTION UNTUK SAVE DATA PLAYER
    public static void SavePlayerData()
    {
        //determine path to save
        string path = Application.dataPath + "/poloData.json";

        //TODO: -Get PlayerData from GameController
        PlayerData data = DataController.Instance.playerData;

        //serialize into binary using formatter
        string dataString = JsonUtility.ToJson(data);

        File.WriteAllText(path, dataString); 

        Debug.Log($"Data has been saved.");
        
    }

    //CALL THIS FUNCTION UNTUK LOAD PLAYER DATA
    public static PlayerData LoadPlayerData()
    {
        //determine filepath
        string path = Application.dataPath + "/poloData.json";

        //check if file is in filepath
        if (File.Exists(path))
        {
            //set player data from JSON
            string JsonString = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(JsonString);  

            //return the data
            return data;
        }
        else
        {
            Debug.Log($"Data not found");
            return null;

        }
    }


}
