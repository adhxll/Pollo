using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public static DataController Instance;
    public LevelDatabase levelDatabase;
    //TODO: Add databases for Achievements and Skins
    public PlayerData playerData;
    List<LevelItemContainer> levels; 
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
            playerData = new PlayerData(levelDatabase);
        }
    }
    void CheckForNewLevel()
    { //check if newly developed level has been added
        List<Level> levels = levelDatabase.allLevels;
        List<LevelItemContainer>  playerDataLevels = playerData.levelData;
        if (levels.Count > playerDataLevels.Count) { 
            for (int i = playerDataLevels.Count-1 ; i < levels.Count-1; i++) { 
                LevelItemContainer previousLvl = playerDataLevels[i];
                playerData.levelData.Add(new LevelItemContainer { levelID = previousLvl.levelID + 1});
            }
        }

    }
    void UpdateLevelData(int levelID, int starCount, int score) {
        //updates level data within playerData instance in singleton
        levels = playerData.levelData; 
        for (int i = 1; i < levels.Count; i++) {
            if (levels[i].levelID == levelID) {
                levels[i].score = score;
                if (levels[i].highScore < score) levels[i].highScore = score;
                if (levels[i].starCount < starCount) levels[i].starCount = starCount;
                // Enable this on certain conditions, e.g. getting certain score and etc. 
                // UnlockNextLevel(levelID); 
            }
        }
    }
    void UnlockNextLevel(int initialLevelID) {
        //unlocks the next level
        for (int i = 1; i < levels.Count; i++) {
            if(levels[i].levelID == initialLevelID && i != levels.Count - 1) 
                levels[i + 1].isUnlocked = true;
        }
    }
}
