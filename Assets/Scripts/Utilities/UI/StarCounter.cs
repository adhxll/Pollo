using System.Collections;
using System.Collections.Generic;
using System; 
using UnityEngine;
using UnityEngine.UI;

public class StarCounter : MonoBehaviour
{
    // This script can be used to fill the stars UI Element (result, levelselect, etc.).
    
    // Gameobject used to store the stars
    [SerializeField] private GameObject[] starSlots = null;
    public int starCount;

    // Set star sprites for filled / empty state in starslots
    [SerializeField] private Sprite filledStarSprite = null;
    [SerializeField] private Sprite emptyStarSprite = null;

    [SerializeField] bool enableShadow = false;

    public void FillStars() {
        try {
            //validation for either SpriteRenderer (world-space) or Image (UI)
            starSlots[0].GetComponent<SpriteRenderer>();
            for (int i = 0; i < starCount; i++)
            {
                var star = starSlots[i].GetComponent<SpriteRenderer>();
                star.color = new Color32(255, 255, 255, 255);
                star.sprite = filledStarSprite;
            }
        }
        catch (Exception) {
            for (int i = 0; i < starCount; i++)
            {
                var star = starSlots[i].GetComponent<Image>();
                star.color = new Color32(253, 196, 25, 255);
                EnableShadow(star.gameObject, enableShadow);
            }
        }
    }

    void EnableShadow(GameObject star, bool shadow)
    {
        if (shadow)
            star.GetComponent<Shadow>().enabled = true;
    }

    public void EmptyStars() {  // Remember to empty the slots before the parent GameObject is set to inactive (kalo destroy gaperlu)
        try
        {
            starSlots[0].GetComponent<SpriteRenderer>();
            for (int i = 0; i < starSlots.Length; i++)
            {
                var star = starSlots[i].GetComponent<SpriteRenderer>();
                star.color = new Color32(90, 133, 113, 255); //darkgreen
                star.sprite = emptyStarSprite; 
            }
        }
        catch (Exception)
        {
            for (int i = 0; i < starSlots.Length; i++)
            {
                var star = starSlots[i].GetComponent<Image>();
                star.color = new Color32(90, 133, 113, 255); //darkgreen
                star.sprite = emptyStarSprite;

            }
        }
       
    }
}
 
