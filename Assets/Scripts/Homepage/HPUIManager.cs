using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Homepage - UI Manager
public class HPUIManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton = null;
    [SerializeField] private GameObject achievementButton = null;
    [SerializeField] private GameObject settingsButton = null;

    public void PlayButton(){
        ButtonController.OnButtonClick(playButton);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LevelSelection);
    }

    public void AchievementButton(){
        ButtonController.OnButtonClick(achievementButton);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.Achievements);
    }

    public void SettingsButton(){
        ButtonController.OnButtonClick(settingsButton);

    }
    
}
