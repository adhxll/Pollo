using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // Update is called once per frame
    public static void GoToGameScene()
    {
        Debug.Log("Successfully clicked");
        SceneManager.LoadScene("GameScene");

    }
    public static void GoToHomePage()
    {
        SceneManager.LoadScene("Homepage");
    }
}