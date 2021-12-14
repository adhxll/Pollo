using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement Database", menuName = "Assets/Database/Achievement")]
public class AchievementDB : ScriptableObject // Database scriptable object achievement
{
    public List<AchievementPop> listAchievement;
    
}
