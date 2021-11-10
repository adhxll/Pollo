using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelItemContainer {
   //LevelItemContainer itu data yang disimpen di dalem levelItem

    public bool isUnlocked;
    private int levelCount; // actual level title
    public int levelScore;
    public int starCount;
    public int score;
    public int highScore;
    public int maxScore;

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
    // Level item itu object level yang ada di gamenya
    public LevelItemContainer data;
    public TMPro.TextMeshPro text; 
    
    public GameObject starContainer;
   
    //awake dan start harus ada karena adu cepat dengan LevelSelectionController dan StarCounter
    private void Awake()
    {
       
        //buat fill stars berdasarkan pencapaian

    }
    void Start()
    {
        text.GetComponent<TMPro.TextMeshPro>().text = data.getLevelCount();
        starContainer.GetComponent<StarCounter>().starCount = data.starCount;
        starContainer.GetComponent<StarCounter>().FillStars(); 
    }
    

}
