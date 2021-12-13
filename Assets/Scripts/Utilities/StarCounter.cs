using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;
using UnityEngine.UI; 
public class StarCounter : MonoBehaviour
{
    // This script can be used to fill the stars UI Element (result, levelselect, etc.).
    
    // Gameobject used to store the stars
    [SerializeField] private GameObject[] StarSlots;
    public int StarCount;
    // Set star sprites for filled / empty state in starslots
    [SerializeField] private Sprite FilledStarSprite = null;
    [SerializeField] private Sprite EmptyStarSprite = null;

    [SerializeField] bool enableShadow = false;

    public void FillStars() {
        try {
            //validation for either SpriteRenderer (world-space) or Image (UI)
            StarSlots[0].GetComponent<SpriteRenderer>();
            for (int i = 0; i < StarCount; i++)
            {
                var star = StarSlots[i].GetComponent<SpriteRenderer>();
                star.color = new Color32(253, 196, 25, 255);
                star.sprite = FilledStarSprite;
                EnableShadow(star.gameObject, enableShadow);
            }
        }
        catch (Exception) {
            for (int i = 0; i < StarCount; i++)
            {
                var star = StarSlots[i].GetComponent<Image>();
                star.color = new Color32(253, 196, 25, 255);
                //star.sprite = FilledStarSprite;
                EnableShadow(star.gameObject, enableShadow);
            }
        }
    }

    void EnableShadow(GameObject star, bool shadow)
    {
        if (shadow)
            star.GetComponent<Shadow>().enabled = true;
    }

    public void EmptyStars() {  //remember to empty the slots before the parent GameObject is set to inactive (kalo destroy gaperlu)
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
        catch (Exception)
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
 
