using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCounter : MonoBehaviour
{
    public GameObject[] StarSlots;
    // Start is called before the first frame update
    public int StarCount;
    public Sprite FilledStarSprite;
    public Sprite EmptyStarSprite;

    public void FillStars() {
        for (int i = 0; i< StarCount; i++)
        {
            var star = StarSlots[i].GetComponent<SpriteRenderer>();
            star.color = new Color32(255, 255, 255, 255); //white
            star.sprite = FilledStarSprite;
        }
    }
    public void EmptyStars() {
        for (int i = 0; i < StarSlots.Length; i++)
        {
            var star = StarSlots[i].GetComponent<SpriteRenderer>();
            star.color = new Color32(90, 133, 113, 255); //darkgreen
            star.sprite = EmptyStarSprite;
            
        }
       
    }
}
 
