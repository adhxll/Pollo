using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static readonly string saveFolder = Application.dataPath + "/Saves/";
    
    //CALL THIS FUNCTION UNTUK SAVE DATA PLAYER
    public static void SavePlayerData()
    {
        //determine path to save
        string path = saveFolder + "/poloLevelData.json";

        //Get PlayerData from GameController
        PlayerData data = DataController.Instance.playerData;

        //convert dictionary to list because it is not JSON serializable
        List<LevelItemContainer> levelList = new List<LevelItemContainer>(); 
        foreach (KeyValuePair<string, LevelItemContainer> kvp in data.levelData) {
            levelList.Add(kvp.Value);
        }
        data.levelList = levelList; 

        //serialize into public vars into json using formatter
        string dataString = JsonUtility.ToJson(data);
        // Debug.Log("data string" + dataString); 
        File.WriteAllText(path, dataString); 

        Debug.Log($"Data has been saved.");
        
    }

    //CALL THIS FUNCTION UNTUK LOAD PLAYER DATA
    public static PlayerData LoadPlayerData()
    {
        //determine filepath
        string path = saveFolder + "/poloLevelData.json";

        //check if file is in filepath
        if (File.Exists(path))
        {
            //set player data from JSON
            string JsonString = File.ReadAllText(path);
            //convert list to dictionary
            Dictionary<string, LevelItemContainer> dict = new Dictionary<string, LevelItemContainer>(); 
            PlayerData data = JsonUtility.FromJson<PlayerData>(JsonString);
            foreach (LevelItemContainer level in data.levelList) {

                string dictKey = DataController.Instance.FormatKey(level.stageID, level.levelID); 
                dict.Add(dictKey, level); 
            }
            data.levelData = dict; 
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
