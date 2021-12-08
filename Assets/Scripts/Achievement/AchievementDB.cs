using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Achievement Database", menuName = "Achievement Database")]
public class AchievementDB : ScriptableObject //database scriptable object achievement
{
    public List<AchievementPop> listAchievement;
    
}
