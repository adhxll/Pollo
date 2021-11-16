using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events; 

public class SceneManagerScript : MonoBehaviour
{
    private string sceneIdentifier;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Homepage" && PlayerPrefs.GetInt("IsFirstTime") == 0) {
            sceneIdentifier = "GameScene";
            SceneInvoke(sceneIdentifier); 
           
        }
    }
    public void SceneInvoke(string sceneName) // invoke scene without delay
    {
        sceneIdentifier = sceneName;
        Invoke(nameof(GoToScene), 0f);
    }

    public void DelayedSceneInvoke(string sceneName)//invoke scene with delay
    {
        sceneIdentifier = sceneName;
        Invoke(nameof(GoToScene), 0.1f);
    }
    public void GoToScene() {
        SceneManager.LoadScene(sceneIdentifier); 
    }

 
  
}
