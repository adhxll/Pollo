using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AchievementPopup : MonoBehaviour // Buat scene popup achievement
{
    [SerializeField] private GameObject capsule = null;
    [SerializeField] private GameObject icon = null, textBg = null;
    [SerializeField] private TextMeshProUGUI text = null, textDescription = null;
    [SerializeField] private AchievementDB achievementDB = null;
    private readonly Color32 purple = new Color32(126, 38, 132, 255),
                        orange = new Color32(255, 200, 113, 255);
    
    private static int id = 1; // Buat milih achievement

    public static void SetAchievementId(int i)
    {
        id = i;
    }

    public void ActivateAchievement()
    {
        var selected = achievementDB.listAchievement[AchievementPopup.id];

        icon.GetComponent<Image>().sprite = selected.IconColoured;
        textBg.GetComponent<Image>().color = orange;
        text.color = purple;
        text.text = selected.text;
        textDescription.color = purple;
        textDescription.text = selected.textDesc;

        StartCoroutine(AnimateAchievement()); 
    }

    public IEnumerator AnimateAchievement()
    {
        AnimationUtilities.Instance.MoveY(capsule, 0, 120);
        yield return new WaitForSeconds(2f);

        AnimationUtilities.Instance.MoveY(capsule, capsule.transform.position.y, -30);
        yield return new WaitForSeconds(0.5f);
        StopCoroutine(AnimateAchievement());
    }

    void Awake()
    {
        
    }

    void Start()
    {
        ActivateAchievement();
    }
}

