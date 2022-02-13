using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Achievement - UI Manager
public class AUIManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsButton = null;
    [SerializeField] private GameObject homeButton = null;

    public void HomeButton(){
        ButtonController.OnButtonClick(homeButton); 
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.Homepage);
    }

    public void SettingsButton(){
        ButtonController.OnButtonClick(settingsButton);
    }
    public void PolloStyle() // Buat set gaya polo
    {
        PolloController.Instance.SetAnimation(10, 0);
        PolloController.Instance.SetActive(true);
    }

}
