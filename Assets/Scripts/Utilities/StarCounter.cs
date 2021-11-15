using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class StarCounter : MonoBehaviour
{
    //This script can be used to fill the stars UI Element (result, levelselect, etc.).
    
    //Gameobject used to store the stars
    public GameObject[] StarSlots;
    public int StarCount;
    //set star sprites for filled / empty state in starslots
    public Sprite FilledStarSprite;
    public Sprite EmptyStarSprite;

    public void FillStars() {
        try {//validation for either SpriteRenderer (world-space) or Image (UI)
            StarSlots[0].GetComponent<SpriteRenderer>();
             for (int i = 0; i < StarCount; i++)
            {
                var star = StarSlots[i].GetComponent<Image>();
                star.color = new Color32(255, 255, 255, 255); //white
                star.sprite = FilledStarSprite;
            }
                }
        catch (MissingComponentException) {
            for (int i = 0; i < StarCount; i++)
            {
                var star = StarSlots[i].GetComponent<Image>();
                star.color = new Color32(255, 255, 255, 255); //white
                star.sprite = FilledStarSprite;
            }
        }
        
    }
    public void EmptyStars() {//remember to empty the slots before the parent GameObject is set to inactive (kalo destroy gaperlu)
        try
        {
            StarSlots[0].GetComponent<SpriteRenderer>();
            for (int i = 0; i < StarSlots.Length; i++)
            {
                var star = StarSlots[i].GetComponent<SpriteRenderer>();
                star.color = new Color32(90, 133, 113, 255); //darkgreen
                star.sprite = EmptyStarSprite; 
            }
        }
        catch (MissingComponentException)
        {
            for (int i = 0; i < StarSlots.Length; i++)
            {
                var star = StarSlots[i].GetComponent<Image>();
                star.color = new Color32(90, 133, 113, 255); //darkgreen
                star.sprite = EmptyStarSprite;

            }
        }
       
    }
}
 
