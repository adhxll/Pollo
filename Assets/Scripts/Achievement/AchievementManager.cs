using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AchievementManager : MonoBehaviour // Buat yg di achievement board
{
    public GameObject icon, textBg;
    public TMPro.TextMeshProUGUI text, description;
    private Sprite iconColoured;
    private Sprite iconBW; 
    public Achievement achievementData;
    public int achievementId;
    public AchievementDB achievementDB; 
 
    private void ActivateAchievement() // Buat set ui achievement
    { 
        if (achievementData.isUnlocked)
        {
            icon.GetComponent<Image>().sprite = iconColoured;
            textBg.GetComponent<Image>().color = new Color32(255, 200, 113, 255);
            text.color = new Color32(126, 38, 132, 255);
            description.color = new Color32(126, 38, 132, 255);
        }
        else 
        {
            icon.GetComponent<Image>().sprite = iconBW;
        }
        
    }
    private void SetupAchievementGameObject() {
        achievementData = DataController.Instance.playerData.achievementData[achievementId];
        iconBW = achievementDB.listAchievement[achievementId].iconGrey;
        iconColoured = achievementDB.listAchievement[achievementId].IconColoured;
        text.text = achievementDB.listAchievement[achievementId].text;
        description.text = achievementDB.listAchievement[achievementId].textDesc;
    }

    private void PolloStyle() // Buat set gaya polo
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
        SetupAchievementGameObject(); 
        ActivateAchievement();
    }
}
