using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveLayout : MonoBehaviour
{
    [SerializeField] private GameObject leftTree = null;

    [SerializeField] private GameObject rightTree = null;

    // Start is called before the first frame update
    void Start()
    {
        // Get current screen width
        var width = Camera.main.orthographicSize * 2.0 * Screen.width / Screen.height;
        var post = (float)width / 6;

        // Repositioning thress object based on screen width
        leftTree.transform.position = new Vector2(-post, 0f);
        rightTree.transform.position = new Vector2(post, 0f);
        //noteBar.GetComponent<SpriteRenderer>().size = new Vector2((float)(width / 1.5), 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
