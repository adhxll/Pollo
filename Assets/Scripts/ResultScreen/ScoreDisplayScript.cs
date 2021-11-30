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
    private string[] successMessages = { "Bru..", "Nice!", "Good!", "Awesome!!" };


    private void Awake()
    {
        getSessionScores();
        SetScoreText();
        SetAccuracyText();
        CalculateStar();
        SetStarIndicator();
        SetSuccessMessage();
    }
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.CoinAdd((int)(score/10f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getSessionScores()
    {
        // The score would be taken from a playerpref called 'SessionScore' that was recorded from the GameScene
        // If an error occured and the playerpref does not exist, it will return 0
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
        // Need to add animation
        if (star > 0) stars[0].GetComponent<Image>().enabled = true;
        if (star > 1) stars[1].GetComponent<Image>().enabled = true;
        if (star > 2) stars[2].GetComponent<Image>().enabled = true;
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
}