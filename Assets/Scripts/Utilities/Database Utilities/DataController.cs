using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController Instance;
    public LevelDatabase levelDatabase;
    public Stages stageDatabase; 
    //TODO: Add databases for Achievements and Skins
    public PlayerData playerData;
    public AchievementDB achievementDB;

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
        Debug.Log("Checking for new Level"); 
        List<Level> levels = levelDatabase.allLevels;
        Dictionary<string, LevelItemContainer>  playerDataLevels = playerData.levelData;
        if (levels.Count > playerDataLevels.Count) { 
            for (int i = playerDataLevels.Count ; i < levels.Count-1; i++) {
                string dictKey = DataController.Instance.FormatKey(levels[i].GetStageID(), levels[i].GetLevelID()); 
                playerData.levelData.Add(dictKey, new LevelItemContainer { 
                    levelID = levels[i].GetLevelID(), 
                    stageID = levels[i].GetStageID()
                });
                Debug.Log("Added level with key: " + dictKey); 
            }
        }
    }

    public void UpdateLevelData(int stageID, int levelID, int starCount, int score, int accuracy) {
        // TODO: Error handling for if it accessed stageID and levelID that does not exist yet
        //updates level data within playerData instance in singleton
        levels = playerData.levelData;
        string dictKey = DataController.Instance.FormatKey(stageID, levelID); 
        levels[dictKey].score = score;
        levels[dictKey].sessionCount += 1; //update session count whenever session is done.
        if (levels[dictKey].highScore < score) levels[dictKey].highScore = score;
        if (levels[dictKey].starCount < starCount) levels[dictKey].starCount = starCount;
        if (levels[dictKey].accuracy < accuracy) levels[dictKey].accuracy = accuracy;
        // Enable this on certain conditions, e.g. getting certain score and etc. 
        // UnlockNextLevel(levelID);
        
    }

    public void UnlockNextLevel(int currentStageID, int currentLevelID) {
        //unlocks the next level
        string dictKey = "";
        if (currentLevelID == stageDatabase.stagesList[0].GetComponent<StageController>().levels.Count
            && currentStageID != stageDatabase.stagesList.Count - 1)//validation for final level and final stage 
            dictKey = DataController.Instance.FormatKey(currentStageID + 1, 1); // unlocks next level of next stage
        else //validation for final stage
            dictKey = DataController.Instance.FormatKey(currentStageID, currentLevelID + 1);

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
            newLevel.levelID = levelDatabase.allLevels[i].GetLevelID();
            newLevel.stageID = levelDatabase.allLevels[i].GetStageID();
            dictKey = DataController.Instance.FormatKey(newLevel.stageID, newLevel.levelID);
            playerData.levelData.Add(dictKey, newLevel);
            Debug.Log("Container ID : " + dictKey + " Locked : " + !playerData.levelData[dictKey].isUnlocked);
        }

        playerData.levelData[FormatKey(1, 1)].isUnlocked = true;

        for (int i = 0; i < achievementDB.listAchievement.Count; i++)
        {
            Achievement ac = new Achievement();
            ac.achievementId = i;
            playerData.achievementData.Add(ac);

        }
    }

    public string FormatKey(int stageID, int levelID) {
        //formats a dictionary key to use in dictionary
        string key = stageID + "-" + levelID;
        return key;
    }
    public void ClearPlayerPrefs() {

        //resets all values to default values, call this in dev mode
        //Jangan lupa kasih default value ketika mau ngecall playerprefs
        PlayerPrefs.DeleteAll(); 

    }
    public void ForceUnlockLevel(int currentStageID, int currentLevelID)
    {
        //unlocks the selected level in devmode, we need to get the stage ID and level ID first from LevelSelection 
        string dictKey = "";
        dictKey = DataController.Instance.FormatKey(currentStageID, currentLevelID);
        levels[dictKey].isUnlocked = true;
    }
    public void ToggleDevMode()
    {
        //used to toggle dev mode
        if (PlayerPrefs.GetInt("DevMode") == 0) PlayerPrefs.SetInt("DevMode", 1);
        else PlayerPrefs.SetInt("DevMode", 0); 

    }
}

