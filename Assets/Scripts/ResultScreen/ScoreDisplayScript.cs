using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreDisplayScript : MonoBehaviour
{
    public int score;
    private int totalNotes;
    private int totalCorrect;
    public TMP_Text scoreObject; // the Score game object on ResultPage scen
    public TMP_Text scoreMessage; // scoreMessage is the success message
    public TMP_Text scoreMessageShadow; // the shadow of scoreMessage, to be removed
    public GameObject[] stars; // the yellow stars inside the Tag GameObject
    public int star = 0;
    private string[] successMessages = { "Bohoo, sucks to be u", "You passed!(barely)", "Good!", "Awesome!!" };


    private void Awake()
    {
        getSessionScores();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetScoreText();
        CalculateStar();
        SetStarIndicator();
        SetSuccessMessage();
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
    }

    void SetScoreText()
    {
        // going to animate this one
        scoreObject.text = score.ToString();
    }

    void SetStarIndicator()
    {
        // Need to add animation
        if (star > 0) stars[0].SetActive(true);
        if (star > 1) stars[1].SetActive(true);
        if (star > 2) stars[2].SetActive(true);
    }

    // Depends on the star, it will show a corresponding message
    void SetSuccessMessage()
    {
        // again, need animation
        scoreMessage.text = successMessages[star];
        scoreMessageShadow.text = successMessages[star];
    }

    void CalculateStar()
    {
        // for pitch detection tolerance
        double firstThreshold = totalNotes * 0.85;
        firstThreshold = Math.Round(firstThreshold);

        double halfThreshold = totalNotes / 2;
        halfThreshold = Math.Round(halfThreshold);

        double thirdThreshold = totalNotes / 3;
        thirdThreshold = Math.Round(thirdThreshold);

        if (totalCorrect >= firstThreshold) star = 3;
        else if (score >= totalCorrect*100 || totalCorrect >= halfThreshold) star = 2;
        else if (totalCorrect >= thirdThreshold) star = 1;
        else star = 0;

    }
}