using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject menuButton;
    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private GameObject nextButton;
    [SerializeField]
    private GameObject settingsButton;

    public void LevelSelectButton()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(menuButton);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LevelSelection);
    }

    public void RestartButton()
    {
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(restartButton);       
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene);
    }

    public void NextButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(nextButton); 
    }

    public void SettingsButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(settingsButton);
    }

}
