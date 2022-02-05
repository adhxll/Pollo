using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Homepage - UI Manager
public class HPUIManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton = null;
    [SerializeField] private GameObject achievementButton = null;
    [SerializeField] private GameObject settingsButton = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        //PlayerPrefs.SetInt(DeveloperMode.DisableAnalytics.ToString(), 0);
        Debug.Log(PlayerPrefs.GetInt(DeveloperMode.DisableAnalytics.ToString()));
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(playButton);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LevelSelection);
    }

    public void AchievementButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(achievementButton);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.Achievements);
    }

    public void SettingsButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(settingsButton);

    }
    
}
