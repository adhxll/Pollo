using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

// Controller Class for ResultScreem
public class RSController : RSElements
{
    // Start is called before the first frame update
    void Start()
    {
        // Doing analytics
        ReportAccuracy();
        DeviceInfoAnalytics(); 
        // Updating database
        UnlockAchievement();
        UpdateLevelData();
        SaveSystem.SavePlayerData();
        AddMoney();
    }

    // Function to add the level data
    // You can get score, stars and accuracy in this class
    void UpdateLevelData()
    {
        int star = app.model.GetStar();
        int score = app.model.GetScore();
        int accuracy = app.model.GetAccuracy();
        DataController.Instance.UpdateLevelData(GameController.Instance.currentStage,GameController.Instance.selectedLevel, star, score, accuracy);
        if (star >= 1) { //unlock if star >= 1
            DataController.Instance.UnlockNextLevel(GameController.Instance.currentStage, GameController.Instance.selectedLevel); 
        }
    }

    public static void TriggerAchievement(int achievementId)
    {
        DataController.Instance.playerData.achievementData[achievementId].isUnlocked = true;
        AchievementPopupController.Instance.achievementList.Add(achievementId);
    }

    // This function must be called BEFORE saving the new session data into the database.
    void UnlockAchievement() 
    {
        int star = app.model.GetStar();
        int score = app.model.GetScore();
        int accuracy = app.model.GetAccuracy();
        string key = app.model.GetCurrentLevelKey();
        if (DataController.Instance.playerData.levelData[key].sessionCount == 0 && DataController.Instance.playerData.achievementData[0].isUnlocked == false) //complete first game
        {
            TriggerAchievement(0);
            // Debug.Log("first game");
        } 
        if (DataController.Instance.playerData.levelData[key].sessionCount > 0 && score > DataController.Instance.playerData.levelData[key].highScore && DataController.Instance.playerData.achievementData[1].isUnlocked == false) //beat own high score
        {
            TriggerAchievement(1);
            // Debug.Log("high score");

        } if (accuracy == 100 && DataController.Instance.playerData.achievementData[2].isUnlocked == false) //not missing a single note in a level
        {
            TriggerAchievement(2);
            // Debug.Log("no miss");

        } if (star == 3 && DataController.Instance.playerData.achievementData[3].isUnlocked == false) //achieve 3 stars
        {
            TriggerAchievement(3);
            // Debug.Log("3 star");

        } if (DataController.Instance.playerData.levelData[key].sessionCount == 4 && DataController.Instance.playerData.achievementData[4].isUnlocked == false) //play the same level 5 times
        {
            TriggerAchievement(4);
        } 
        AchievementPopupController.Instance.LoadAchievementPopup();
    }

    void AddMoney()
    {
        int score = app.model.GetScore();
        if (score > 10)
        {
            GameController.Instance.coinController.CoinAdd((int)(score / 10f));
        }
    }

    #region Analytics Function

    Dictionary<string, object> GetLevelParameters()
    {
        Dictionary<string, object> customParams = new Dictionary<string, object>();
        customParams.Add("stage", GameController.Instance.currentStage);
        customParams.Add("level", GameController.Instance.selectedLevel);

        return customParams;
    }

    void ReportAccuracy()
    {
        if (PlayerPrefs.GetInt(DeveloperMode.DisableAnalytics.ToString()) == 1)
        {
            var customParams = GetLevelParameters();
            customParams.Add("accuracy", app.model.GetAccuracy());
            customParams.Add("timesPlayed", DataController.Instance.playerData.levelData[app.model.GetCurrentLevelKey()].sessionCount);
            var analytics = Analytics.CustomEvent("Accuracy", customParams);
        }
        else
        {
            Debug.Log("Report Accuracy not Uploaded to Analytics");
        }
    }

    void DeviceInfoAnalytics()
    {
        if (PlayerPrefs.GetInt(DeveloperMode.DisableAnalytics.ToString()) == 1)
        {
            // Debug.Log("Sending Device Info Analytics..."); 
            var result = Analytics.CustomEvent("Device Information with Score Delay", new Dictionary<string, object> {
                {"Device Type", SystemInfo.deviceType },
                {"Device Model",  SystemInfo.deviceModel },
                {"Delay Settings", PlayerPrefs.GetFloat(SettingsList.Delay.ToString())},
                {"Session Count" ,  DataController.Instance.playerData.levelData[app.model.GetCurrentLevelKey()].sessionCount},
                {"Accuracy: " , app.model.GetAccuracy() }
            });
            // Debug.Log("Analytics Result: " + result);
        }
        else
        {
            Debug.Log("Device Info Analytics not Uploaded to Analytics");
        }
    }
    #endregion; 
}
