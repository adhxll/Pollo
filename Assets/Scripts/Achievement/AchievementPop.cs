using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Assets/Achievement")]
public class AchievementPop : ScriptableObject // Buat store scriptable object achievement
{
    public Sprite IconColoured, iconGrey;
    public string text, textDesc;
    public Color32 textBackg;
    public int achievementId;
}
