using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject achievementButton;
    [SerializeField]
    private GameObject settingsButton;

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
