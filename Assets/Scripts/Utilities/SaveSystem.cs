using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    //CALL THIS FUNCTION UNTUK SAVE DATA LEVEL
    public static void SavePlayerData()
    {
     
        //determine path to save
        string path = Application.dataPath + "/poloData.json";

        //TODO: -Get PlayerData from GameController
        PlayerData data = GameController.Instance.playerData;

        //serialize into binary using formatter
        string dataString = JsonUtility.ToJson(data);

        File.WriteAllText(path, dataString); 

        Debug.Log($"Data has been saved.");
        
    }

    //CALL THIS FUNCTION UNTUK LOAD LEVEL DATA
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
            
            Debug.Log("Save file found and loaded.");
            int i = 0;
            foreach(LevelItemContainer level in data.levelData){
                i++; 
            }
            Debug.Log("Level Count : " + i); 
            //return the data
            return data;
        }
        else
        {
            Debug.Log($"Data not found");
            return null;




        }
    }
    // Start is called before the first frame update

}
