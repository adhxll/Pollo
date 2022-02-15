using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementMenuGenerator : MonoBehaviour
{
    //used to generate achievement entries inside of the achievement scene
    public AchievementDB achievementDB;
    public AchievementManager achievementManager;
    public GameObject layoutGroup; 
    private void Awake()
    {
        for (int i = 0; i < achievementDB.listAchievement.Count; i++)
        {
            achievementManager.achievementId = i;
            AchievementManager newAchievement = Instantiate(achievementManager);
            newAchievement.transform.parent = layoutGroup.transform;
            newAchievement.transform.localScale = new Vector3(1f, 1f); 
        }
    }
    void Start()
    {

    }
}

  
