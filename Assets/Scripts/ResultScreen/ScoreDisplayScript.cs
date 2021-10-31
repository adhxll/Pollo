using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplayScript : MonoBehaviour
{
    public int score;
    public TMP_Text scoreObject;
    public TMP_Text scoreMessage;
    public TMP_Text scoreMessageShadow;
    public GameObject[] stars;
    public int star = 0;
    private string[] successMessages = { "Bohoo, sucks to be u", "You passed!(barely)", "Good!", "Awesome!!" };


    private void Awake()
    {
        setScore();
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

    void setScore()
    {
        // The score would be taken from a playerpref called 'SessionScore' that was recorded from the GameScene
        // If an error occured and the playerpref does not exist, it will return 0
        score = PlayerPrefs.GetInt("SessionScore", 90);
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

        if (score > 80) star = 3;
        else if (score > 50) star = 2;
        else if (score > 20) star = 1;
        else star = 0;

    }
}