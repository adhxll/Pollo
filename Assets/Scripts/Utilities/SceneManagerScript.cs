using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public static void GoToGameScene() {
        SceneManager.LoadScene("GameScene");
        
    }
    public static void GoToHomePage() {
        SceneManager.LoadScene("Homepage");
    }
}
