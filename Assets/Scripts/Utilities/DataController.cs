using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController Instance;
    public LevelDatabase levelDatabase;
    //TODO: Add databases for Achievements and Skins
    public PlayerData playerData;
    Dictionary<string, LevelItemContainer> levels; 
    private void Awake()
    {
        StartSingleton(); 
    }
    void Start()
    {
        InitializePlayerData(); 
    }

    void StartSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void InitializePlayerData() {
        if (SaveSystem.LoadPlayerData() != null)
        {
            playerData = SaveSystem.LoadPlayerData();
            CheckForNewLevel();
        }
        else
        {
            GenerateInitialvalue(); 
        }
    }
    public void CheckForNewLevel()
    { //check if newly developed level has been added
        List<Level> levels = levelDatabase.allLevels;
        Dictionary<string, LevelItemContainer>  playerDataLevels = playerData.levelData;
        if (levels.Count > playerDataLevels.Count) { 
            for (int i = playerDataLevels.Count-1 ; i < levels.Count-1; i++) {
                string dictKey = DataController.Instance.FormatKey(levels[i].GetStageID(), levels[i].GetLevelID()); 
                playerData.levelData.Add(dictKey, new LevelItemContainer { 
                    levelID = levels[i].GetStageID(), 
                    stageID = levels[i].GetLevelID()
                }); 
            }
        }

    }
    public void UpdateLevelData(int stageID, int levelID, int starCount, int score, int accuracy) {
        // TODO: Error handling for if it accessed stageID and levelID that does not exist yet
        //updates level data within playerData instance in singleton
        levels = playerData.levelData;
        string dictKey = DataController.Instance.FormatKey(stageID, levelID); 
        levels[dictKey].score = score;
        if (levels[dictKey].highScore < score) levels[dictKey].highScore = score;
        if (levels[dictKey].starCount < starCount) levels[dictKey].starCount = starCount;
        if (levels[dictKey].accuracy < accuracy) levels[dictKey].accuracy = accuracy;
        // Enable this on certain conditions, e.g. getting certain score and etc. 
        // UnlockNextLevel(levelID);
        
    }
    public void UnlockNextLevel(int currentStageID, int currentLevelID) {
        //unlocks the next level
        //TODO: Validation for final level
        string dictKey = DataController.Instance.FormatKey(currentStageID, currentLevelID + 1); 
        levels[dictKey].isUnlocked = true; 
       
    }
    public void GenerateInitialvalue()
    {
        //populate onboarding and first level
        LevelItemContainer level = new LevelItemContainer();
        string dictKey = DataController.Instance.FormatKey(level.stageID, level.levelID); 
        playerData.levelData.Add(dictKey, level);

        level = new LevelItemContainer();
        level.levelID = 1;
        dictKey = level.stageID + "-" + level.levelID;
        level.isUnlocked = true;
        playerData.levelData.Add(dictKey, level);

        //populate levels with empty values
        for (int i = 2; i < levelDatabase.allLevels.Count; i++)
        {
            LevelItemContainer newLevel = new LevelItemContainer();
            newLevel.levelID = i;
            newLevel.stageID = levelDatabase.allLevels[i].GetStageID();
            newLevel.starCount = Random.Range(0, 4);
            dictKey = DataController.Instance.FormatKey(newLevel.stageID, newLevel.levelID); 
            playerData.levelData.Add(dictKey, newLevel);
            Debug.Log("Container ID : " + dictKey + " Star Count : " + playerData.levelData[dictKey].starCount);
        }

    }
    public string FormatKey(int stageID, int levelID) {
        //formats a dictionary key to use in dictionary
        string key = stageID + "-" + levelID;
        return key; 
    }
}

