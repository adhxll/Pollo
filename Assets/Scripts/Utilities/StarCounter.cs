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
    void Start()
    {
        for (int i = 3; i > starCount; i--)
        {
            starSlots[i - 1].GetComponent<SpriteRenderer>().sprite = purpleStar;
        }
    }
}
 
