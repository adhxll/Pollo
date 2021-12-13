using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreDisplayScript : MonoBehaviour
{
    private int score;
    private int totalNotes;
    private int totalCorrect;
    private int accuracy;
    [SerializeField]
    private TMP_Text scoreMessageObject = null;
    [SerializeField]
    private TMP_Text scoreObject = null; // the Score game object on ResultPage scene
    [SerializeField]
    private TMP_Text accuracyObject = null; // the Score game object on ResultPage scene
    [SerializeField]
    private GameObject[] stars = null; // the yellow stars inside the Tag GameObject
    [SerializeField]
    private int star = 0;
    private string[] successMessages = { "Try again!", "Nice!", "Good!", "Awesome!!" };


    private void Awake()
    {
        GetSessionScores();
        SetScoreText();
        SetAccuracyText();
        CalculateStar();
        SetStarIndicator();
        SetSuccessMessage();
        SaveLevelData();
    }
    // Start is called before the first frame update
    void Start()
    {
        AddMoney();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function to add the level data
    // Currently empty
    // You can get score, stars and accuracy in this class
    void SaveLevelData()
    {
        DataController.Instance.UpdateLevelData(GameController.Instance.currentStage,GameController.Instance.selectedLevel, star, score, accuracy);

    }

    void AddMoney()
    {
        if (score > 10)
        {
            GameController.Instance.CoinAdd((int)(score / 10f));
        }
    }

    void GetSessionScores()
    {
        // The score would be taken from a playerpref called 'SessionScore' that was recorded from the GameScene
        // If an error occured and the playerpref does not exist, it will return a default value (0 or 1, depending on each variable)
        score = PlayerPrefs.GetInt("SessionScore", 0);
        totalNotes = PlayerPrefs.GetInt("SessionTotalNotes", 1);
        totalCorrect = PlayerPrefs.GetInt("SessionCorrectNotes", 0);
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
}