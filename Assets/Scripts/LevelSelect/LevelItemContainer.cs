using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelItemContainer : MonoBehaviour
{
    //LevelItemContainer itu data yang disimpen di dalem levelItem 

    //TODO: - Reformat level data
    public bool isUnlocked;
    private int levelCount; //level number
    public int levelScore;
    public int starCount;
    public int score;
    public int highScore;
    public int maxScore;

    //TODO: - Reformat song data
    public char song;
    public int bpm;
    public string[] songMelodies;
    public int[] segmentMelodies;
    public LevelItemContainer(int levelCount)
    {
        this.levelCount = levelCount;
        isUnlocked = false;
        levelScore = 0;
        starCount = 0;
        score = 0;
        highScore = 0;
        maxScore = 0;
        song = 'b';
        bpm = 0;
        songMelodies = new string[14];
        segmentMelodies = new int[14];
    }
    public string getLevelCount()
    {
        return this.levelCount.ToString();
    }
}
