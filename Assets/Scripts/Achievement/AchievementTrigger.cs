using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementTrigger : MonoBehaviour

{
    void Start()
    {
        
    }

   public static void TriggerAchievement(int achievementId)
    {
        DataController.Instance.playerData.achievementData[achievementId].isUnlocked = true;
    }
}
