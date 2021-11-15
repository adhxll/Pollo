using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject Icon, TextBg;
    public TMPro.TextMeshProUGUI Text, TextDescription;
    public Sprite IconColoured;
    public bool isUnlocked;
 
    public void ActivateAchievement()
    {
        if (isUnlocked )
        {
            Icon.GetComponent<Image>().sprite = IconColoured;
            TextBg.GetComponent<Image>().color = new Color32(255, 200, 113, 255);
            Text.color = new Color32(126, 38, 132, 255);
            TextDescription.color = new Color32(126, 38, 132, 255);
        }
    
    }

    void Awake()
    {
        ActivateAchievement();
    }


 
}
