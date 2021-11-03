using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instace;
    public AudioSource hitSFX;
    //public AudioSource missSFX;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshPro comboText;

    static int comboScore;
    static int totalScore;

    // Start is called before the first frame update
    void Start()
    {
        Instace = this;
        comboScore = 0;
        totalScore = 0;
    }

    public static void Hit()
    {
        comboScore += 1;
        totalScore += (98 + comboScore * 2); //So that the default score is 100

        ParticleController.Instance.EmitParticle(1);
        AnimationManager.Instace.AnimateHit(Instace.scoreText.gameObject, 0.25f);
        AnimationManager.Instace.AnimateHit(Instace.comboText.gameObject, 0.25f);
        //Instace.hitSFX.Play();
    }

    public static void Miss()
    {
        comboScore = 0;
        //Instace.missSFX.Play();
    }

    public void Reset()
    {
        comboScore = 0;
        totalScore = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        scoreText.text = totalScore.ToString();
        comboText.text = comboScore.ToString();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("SessionScore", totalScore);
    }
}
