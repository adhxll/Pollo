using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Model Class for ResultScreem
public class RSModel : RSElements
{
    private int score;
    private int totalNotes;
    private int totalCorrect;
    private int totalWrong;
    private int accuracy;
    private int star = 0;
    private string[] successMessages = { "Try again!", "Good!", "Nice!", "Awesome!"};
    private string currentLevelKey;

    public int GetScore(){ return score; }
    public int GetTotalNotes(){ return totalNotes; }
    public int GetTotalCorrect(){ return totalCorrect; }
    public int GetTotalWrong(){ return totalWrong; }
    public int GetAccuracy(){ return accuracy; }
    public int GetStar(){ return star; }
    public string GetSuccessMessage(){ return successMessages[GetStar()];}
    public string GetCurrentLevelKey(){ return currentLevelKey; }

    void Awake()
    {
        SetSessionData();
    }

    // The setter is only this one big giant function because we won't set any property of this class manually
    void SetSessionData()
    {
        // The score would be taken from a playerpref called 'SessionScore' that was recorded from the GameScene
        // If an error occured and the playerpref does not exist, it will return a default value (0 or 1, depending on each variable)
        score = PlayerPrefs.GetInt("SessionScore", 0);
        Debug.Log("SESSION SCORE: " + score);
        totalNotes = PlayerPrefs.GetInt("SessionTotalNotes", 1);
        totalCorrect = PlayerPrefs.GetInt("SessionCorrectNotes", 0);
        totalWrong = totalNotes - totalCorrect;
        accuracy = PlayerPrefs.GetInt("SessionAccuracy", 0);
        star = CalculateStar();
        currentLevelKey = DataController.Instance.FormatKey(GameController.Instance.currentStage, GameController.Instance.selectedLevel);
    }

    // Sets stars based on accuracy
    int CalculateStar()
    {
        int s = 0;
        // havent decide on what happens if the accuracy is 100
        if (accuracy >= 90) s = 3;
        else if (accuracy >= 60) s = 2;
        else if (accuracy >= 30) s = 1;
        
        return s;
    }
}
