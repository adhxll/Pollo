using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelItemContainer {
   //LevelItemContainer itu data yang disimpen di dalem levelItem

    //level data 
    public bool isUnlocked;
    private int levelCount; //level number
    public int levelScore;
    public int starCount;
    public int score;
    public int highScore;
    public int maxScore;

    //song data
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
    public string getLevelCount() { //used for displaying only!!! don't fuck with this please!!!!!!
        return this.levelCount.ToString(); 
    }
}
public class LevelItem : MonoBehaviour
{
    //data to fill the level item
    public LevelItemContainer data;
    //containers for showing the data
    public TMPro.TextMeshPro LevelCountText; 
    public GameObject StarContainer;
    public GameObject LockUnlockCircle;
    public Sprite UnlockedCircleSprite; 
    void Start()
    {
        //setup the data into container
        LevelCountText.GetComponent<TMPro.TextMeshPro>().text = data.getLevelCount();
        StarContainer.GetComponent<StarCounter>().StarCount = data.starCount;
        StarContainer.GetComponent<StarCounter>().FillStars();
        if (data.isUnlocked) LockUnlockCircle.GetComponent<SpriteRenderer>().sprite = UnlockedCircleSprite; 
    }
    

}
