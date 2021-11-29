using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instace;

    //[SerializeField]
    //AudioSource hitSFX = null;

    //[SerializeField]
    //AudioSource missSFX = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI comboText = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI accuracyText = null;

    [SerializeField]
    private GameObject hitBadge = null;

    
    static public string missMessage = "";
    private string[] hitMessage = { "Perfect", "Eggcellent", "Awesome", "Great", "Good" };
    //private string[] missMessage = { "Miss", "Oopsy", "Aw...", "Sad Trombone", "Uh-oh"};

    static float correctNotes = 0;
    static float totalNotes = 0;
    static int accuracy = 0;

    static int comboScore = 0;
    static int wrongScore = 0;
    static int totalScore = 0;

    static bool badgeShowed = false;
    double timer = 0;
    double timeSinceShowed = 0;

    void Start()
    {
        Instace = this;
        Reset();
    }

    public static void Hit()
    {
        comboScore++;
        correctNotes++;
        totalNotes++;

        wrongScore = 0;
        totalScore += (98 + comboScore * 2);

        ParticleController.Instance.EmitParticle(1, ParticleController.Indicator.Hit);
        PolloController.Instance.SetAnimation(comboScore, wrongScore);

        Instace.HitBadge(true);

        AnimationUtilities.Instance.PunchScale(Instace.scoreText.gameObject);
        AnimationUtilities.Instance.PunchScale(Instace.comboText.gameObject);
        AnimationUtilities.Instance.PunchScale(Instace.accuracyText.gameObject);

        //Instace.hitSFX.Play();
    }

    public static void Miss()
    {
        wrongScore++;
        totalNotes++;
        comboScore = 0;

        Instace.HitBadge(false);

        PolloController.Instance.SetAnimation(comboScore, wrongScore);
        ParticleController.Instance.EmitParticle(1, ParticleController.Indicator.Miss);

        //Instace.missSFX.Play();
    }

    public void HitBadge(bool hit)
    {
        System.Random rnd = new System.Random();
        int hitIndex = rnd.Next(0, hitMessage.Length - 1);
        //int missIndex = rnd.Next(0, missMessage.Length - 1);
        var messageText = hitBadge.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        // Determine badge style based on hit or miss
        if (hit)
        {
            messageText.SetText(hitMessage[hitIndex]);
            hitBadge.GetComponent<Image>().color = new Color32(151, 174, 124, 255);
        }

        else
        {
            messageText.SetText(missMessage);
            hitBadge.GetComponent<Image>().color = new Color32(247, 86, 82, 255);
        }
        
        if (badgeShowed)
        {
            // Animate punch if badge is shown
            AnimationUtilities.Instance.PunchScale(hitBadge);
            timer = AudioSettings.dspTime;
        }
        else
        {
            // Animate slide position if badge isn't shown
            AnimationUtilities.Instance.LocalMoveY(hitBadge, 25f, 0.25f);
            badgeShowed = true;
            timer = AudioSettings.dspTime;
        }
    }

    public void Reset()
    {
        badgeShowed = false;
        timeSinceShowed = 0;
        timer = 0;

        correctNotes = 0;
        totalNotes = 0;
        comboScore = 0;
        totalScore = 0;
        wrongScore = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        scoreText.text = totalScore.ToString();
        comboText.text = comboScore.ToString();

        if (correctNotes == 0 && totalNotes == 0)
            accuracyText.text = "0";
        else
            accuracy = (int)(correctNotes / totalNotes * 100);
            accuracyText.text = accuracy.ToString("0");

        // Track how long the badge has shown
        timeSinceShowed = AudioSettings.dspTime - timer;

        // If the badge has shown for more than 2s and nothing else is happening, hide the badge
        if (badgeShowed && timeSinceShowed > 2f)
        {
            badgeShowed = false;
            AnimationUtilities.Instance.LocalMoveY(hitBadge, 60f, 0.25f);
        }
    }


    // Temporary function to pass the session's data
    private void OnDestroy()
    {
        PlayerPrefs.SetInt("SessionScore", totalScore);
        PlayerPrefs.SetInt("SessionTotalNotes", (int)totalNotes);
        PlayerPrefs.SetInt("SessionCorrectNotes", (int)correctNotes); 
        PlayerPrefs.SetInt("SessionAccuracy", accuracy);
    }
}
