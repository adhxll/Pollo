using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AchievementPopup : MonoBehaviour //buat scene popup achievement
{
    [SerializeField] private GameObject Capsule;
    [SerializeField] private GameObject Icon = null, TextBg = null;
    [SerializeField] private TMPro.TextMeshProUGUI Text = null, TextDescription = null;
    [SerializeField] private Sprite IconColoured = null;
    [SerializeField] private AchievementDB achievementDB = null;
    private readonly Color32 purple = new Color32(126, 38, 132, 255),
                        orange = new Color32(255, 200, 113, 255);
    
    private static int id = 1; //buat milih achievement

    public static void SetAchievementId(int i)
    {
        id = i;
    }

    public void ActivateAchievement()
    {
            var selected = achievementDB.listAchievement[AchievementPopup.id];

            Icon.GetComponent<Image>().sprite = selected.IconColoured;
            TextBg.GetComponent<Image>().color = orange;
            Text.color = purple;
            Text.text = selected.text;
            TextDescription.color = purple;
            TextDescription.text = selected.textDesc;

            StartCoroutine(AnimateAchievement());
           
    }

    public IEnumerator AnimateAchievement()
    {
        AnimationUtilities.Instance.MoveY(Capsule, 0, 120);
        yield return new WaitForSeconds(2f);

        AnimationUtilities.Instance.MoveY(Capsule, Capsule.transform.position.y, -30);
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

