using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalController : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshPro levelText;
    public TMPro.TextMeshPro scoreText;
    public string levelValue;
    public string scoreValue;
    
    public void SetValues()
    {
        levelText.text = levelValue;
        scoreText.text = scoreValue;
    }
    
}
