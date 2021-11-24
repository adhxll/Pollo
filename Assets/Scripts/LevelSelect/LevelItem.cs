using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        LevelCountText.GetComponent<TMPro.TextMeshPro>().text = data.levelID.ToString(); 
        StarContainer.GetComponent<StarCounter>().StarCount = data.starCount;
        StarContainer.GetComponent<StarCounter>().FillStars();
        if (data.isUnlocked) LockUnlockCircle.GetComponent<SpriteRenderer>().sprite = UnlockedCircleSprite; 
    }
    

}
