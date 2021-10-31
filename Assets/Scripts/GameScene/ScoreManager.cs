using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instace;
    public AudioSource hitSFX;
    //public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
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
        totalScore += (98 + comboScore * 2);

        AnimationManager.Instace.AnimateHit(Instace.scoreText.gameObject, 0.25f);
        AnimationManager.Instace.AnimateHit(Instace.comboText.gameObject, 0.25f);
        //Instace.hitSFX.Play();
    }

    public static void Miss()
    {
        comboScore = 0;
        //Instace.missSFX.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        scoreText.text = totalScore.ToString();
        comboText.text = comboScore.ToString();
    }
}
