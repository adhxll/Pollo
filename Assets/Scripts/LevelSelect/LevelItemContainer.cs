using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelItemContainer
{
    //LevelItemContainer itu data yang disimpen di dalem levelItem 

    //TODO: - Reformat level data
    public bool isUnlocked = false;
    public int levelID = 0; //level number
    public int levelScore = 0;
    public int starCount = 0;
    public int score = 0;
    public int highScore = 0;
    public int maxScore = 0;

    
}
