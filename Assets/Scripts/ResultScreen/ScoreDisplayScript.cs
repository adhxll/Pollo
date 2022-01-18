using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreDisplayScript : MonoBehaviour
{
    private int score;
    private int totalNotes;
    private int totalCorrect;
    private int totalWrong;
    private int accuracy;

    [SerializeField] private TMP_Text scoreMessageObject = null;
    [SerializeField] private TMP_Text scoreObject = null; // the Score game object on ResultPage scene
    [SerializeField] private TMP_Text accuracyObject = null; // the Score game object on ResultPage scene
    [SerializeField] private GameObject[] stars = null; // the yellow stars inside the Tag GameObject
    [SerializeField] private int star = 0;

    private string[] successMessages = { "Try again!", "Good!", "Nice!", "Awesome!"};

    String currentLevelKey = DataController.Instance.FormatKey(GameController.Instance.currentStage, GameController.Instance.selectedLevel);

    public int getScore(){
        return score;
    }
    public int getTotalNotes(){
        return totalNotes;
    }
    public int getTotalCorrect(){
        return totalCorrect;
    }
    public int getTotalWrong(){
        return totalWrong;
    }
    public int getAccuracy(){
        return accuracy;
    }
    
    private void Awake()
    {
        GetSessionScores();
        SetScoreText();
        SetAccuracyText();
        CalculateStar();
        SetStarIndicator();
        SetSuccessMessage();
        UpdateLevelData();
        UnlockAchievement();
        SaveSystem.SavePlayerData();
    }

    // Start is called before the first frame update
    void Start()
    {
        RightOrWrongNoteSequenceAnalytics();
        DeviceInfoAnalytics(); 
        AddMoney();
    }

    // Function to add the level data
    // You can get score, stars and accuracy in this class
    void UpdateLevelData()
    {
        DataController.Instance.UpdateLevelData(GameController.Instance.currentStage,GameController.Instance.selectedLevel, star, score, accuracy);
        if (star >= 1) { //unlock if star >= 1
            DataController.Instance.UnlockNextLevel(GameController.Instance.currentStage, GameController.Instance.selectedLevel); 
        }
    }

    void AddMoney()
    {
        if (score > 10)
        {
            GameController.Instance.coinMechanism.CoinAdd((int)(score / 10f));
        }
    }

    void GetSessionScores()
    {
        // The score would be taken from a playerpref called 'SessionScore' that was recorded from the GameScene
        // If an error occured and the playerpref does not exist, it will return a default value (0 or 1, depending on each variable)
        score = PlayerPrefs.GetInt("SessionScore", 0);
        totalNotes = PlayerPrefs.GetInt("SessionTotalNotes", 1);
        totalCorrect = PlayerPrefs.GetInt("SessionCorrectNotes", 0);
        totalWrong = totalNotes - totalCorrect;
        accuracy = PlayerPrefs.GetInt("SessionAccuracy", 0);
    }

    void SetScoreText()
    {
        // going to animate this one
        scoreObject.text = score.ToString();
    }

    void SetAccuracyText()
    {
        // going to animate this one
        accuracyObject.text = accuracy.ToString() + "%";
    }

    void SetStarIndicator()
    {
        StartCoroutine(StarAnimation());
    }

    // Depends on the star, it will show a corresponding message
    void SetSuccessMessage()
    {
        // again, need animation
        scoreMessageObject.text = successMessages[star];
    }

    void CalculateStar()
    {
        // havent decide on what happens if the accuracy is 100
        if (accuracy >= 90) star = 3;
        else if (accuracy >= 60) star = 2;
        else if (accuracy >= 30) star = 1;
        else star = 0;

    }

    public static void TriggerAchievement(int achievementId)
    {
        DataController.Instance.playerData.achievementData[achievementId].isUnlocked = true;
        AchievementPopupController.Instance.achievementList.Add(achievementId);
    }

    void UnlockAchievement() 
    {

        if (DataController.Instance.playerData.levelData[currentLevelKey].sessionCount == 0 && DataController.Instance.playerData.achievementData[0].isUnlocked == false) //complete first game
        {
            TriggerAchievement(0);
            Debug.Log("first game");

        } if (score > DataController.Instance.playerData.levelData[currentLevelKey].highScore && DataController.Instance.playerData.achievementData[1].isUnlocked == false &&
            DataController.Instance.playerData.levelData[currentLevelKey].sessionCount > 0) //beat own high score
        {
            TriggerAchievement(1);
            Debug.Log("high score");

        } if (accuracy == 100 && DataController.Instance.playerData.achievementData[2].isUnlocked == false) //not missing a single not in a level
        {
            TriggerAchievement(2);
            Debug.Log("no miss");

        } if (star == 3 && DataController.Instance.playerData.achievementData[3].isUnlocked == false) //achieve 3 stars
        {
            TriggerAchievement(3);
            Debug.Log("3 star");

        } if (DataController.Instance.playerData.levelData[currentLevelKey].sessionCount == 4 && DataController.Instance.playerData.achievementData[4].isUnlocked == false) //play the same level 5 times
        {
            TriggerAchievement(4);
        } 
        AchievementPopupController.Instance.LoadAchievementPopup();
    }
    IEnumerator StarAnimation(){
        // Need to add animation
        yield return new WaitForSeconds(1f);
        if (star > 0) {
            stars[0].GetComponent<Image>().enabled = true;
            AnimationUtilities.Instance.PunchScale(stars[0]);
            AudioController.Instance.PlaySound(SoundNames.star1);
        }
        yield return new WaitForSeconds(1f);
        if (star > 1) {
            stars[1].GetComponent<Image>().enabled = true;
            AnimationUtilities.Instance.PunchScale(stars[1]);
            AudioController.Instance.PlaySound(SoundNames.star2);
        }
        yield return new WaitForSeconds(1f);
        if (star > 2) {
            stars[2].GetComponent<Image>().enabled = true;
            AnimationUtilities.Instance.PunchScale(stars[2]);
            AudioController.Instance.PlaySound(SoundNames.star3);
        }
    }
    #region Analytics Functions
    void RightOrWrongNoteSequenceAnalytics()
    {
        Debug.Log("Sending Note Sequence Analytics...");
        var result = Analytics.CustomEvent(
            "Right or Wrong Notes Sequence",
            new Dictionary<string, object>{
                {"Stage", GameController.Instance.currentStage},
                {"Level", GameController.Instance.selectedLevel},
                {"Correct Notes", getTotalCorrect()},
                {"Wrong Notes", getTotalWrong()}
            }
        );
        Debug.Log("Analytics Result: " + result);
    }
    void DeviceInfoAnalytics()
    {
        Debug.Log("Sending Device Info Analytics..."); 
        var result = Analytics.CustomEvent("Device Information with Score Delay", new Dictionary<string, object> {
            {"Device Type", SystemInfo.deviceType },
            {"Device Model",  SystemInfo.deviceModel },
            {"Delay Settings", PlayerPrefs.GetFloat(SettingsList.Delay.ToString())},
            {"Session Count" ,  DataController.Instance.playerData.levelData[currentLevelKey].sessionCount},
            {"Accuracy: " , accuracy}
        });
        Debug.Log("Analytics Result: " + result);
    }
    #endregion; 
}