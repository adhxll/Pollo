using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCounter : MonoBehaviour
{
    public GameObject[] starSlots;
    // Start is called before the first frame update
    public int starCount;
    public Sprite yellowStar;
    public Sprite purpleStar;

    public void FillStars() {
        for (int i = 0; i< starCount; i++)
        {
            var star = starSlots[i].GetComponent<SpriteRenderer>();
            star.color = new Color32(255, 255, 255, 255); //white
            star.sprite = yellowStar;
        }
    }
    public void EmptyStars() {
        for (int i = 0; i < starSlots.Length; i++)
        {
            var star = starSlots[i].GetComponent<SpriteRenderer>();
            star.color = new Color32(90, 133, 113, 255); //darkgreen
            star.sprite = purpleStar;
            
        }
       
    }
}
 
