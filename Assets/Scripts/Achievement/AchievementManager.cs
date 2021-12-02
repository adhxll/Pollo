using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AchievementManager : MonoBehaviour //buat yg di achievement board
{
    public GameObject Icon, TextBg;
    public TMPro.TextMeshProUGUI Text, TextDescription;
    public Sprite IconColoured;
    public Achievement achievementData;
 
    public void ActivateAchievement() //buat set ui achievement
    {
        if (achievementData.isUnlocked )
        {
            Icon.GetComponent<Image>().sprite = IconColoured;
            TextBg.GetComponent<Image>().color = new Color32(255, 200, 113, 255);
            Text.color = new Color32(126, 38, 132, 255);
            TextDescription.color = new Color32(126, 38, 132, 255);

        }
    }

    public void PolloStyle() //buat set gaya polo
    {
        PolloController.Instance.SetAnimation(10, 0);
        PolloController.Instance.SetActive(true);
    }

   void Start()
    {
        Invoke("PolloStyle", 1f);
    }

    void Awake()
    {
        ActivateAchievement();
     
    }


 
}
