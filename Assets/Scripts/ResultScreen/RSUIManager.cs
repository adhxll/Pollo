using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSUIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuButton = null;
    [SerializeField] private GameObject restartButton = null;
    [SerializeField] private GameObject settingsButton = null;
    [SerializeField] private Button levelSelectButton = null;
    [SerializeField] private Button nextButton = null;
    private bool isNextLevel = false;
    private bool isNextStage = false;

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
        AnimationUtilities.AnimateButtonPush(nextButton.gameObject);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene); 
    }

    public void SettingsButton(){
        AudioController.Instance.PlaySound(SoundNames.click);
        AnimationUtilities.AnimateButtonPush(settingsButton);
    }

    private void Start()
    {
        InitializeNextButton();
        InitializeLevelSelectButton();
    }
    void changeGameSceneState()
    {
        if (isNextLevel)
        {
            GameController.Instance.selectedLevel = +1;
        }
        else
        {
            GameController.Instance.selectedLevel = 1;
            GameController.Instance.currentStage += 1;
        }
    }

    void InitializeNextButton()
    {
        nextButton.interactable = false;
        // next level dict key
        string nextLevelKey = DataController.Instance.FormatKey(GameController.Instance.currentStage, GameController.Instance.selectedLevel + 1);
        // next stage dict key
        string nextStageKey = DataController.Instance.FormatKey(GameController.Instance.currentStage + 1, 1);
        Debug.Log("" + nextLevelKey);
        Debug.Log("" + nextStageKey);

        Dictionary<string, LevelItemContainer> levels = DataController.Instance.playerData.levelData;

        if (levels.ContainsKey(nextLevelKey))
        {
            if (levels[nextLevelKey].isUnlocked)
            {
                nextButton.interactable = true;
                isNextLevel = true;
                nextButton.onClick.AddListener(changeGameSceneState);
            }
        }
        else if (levels.ContainsKey(nextStageKey)) //if the next stage is available
        {
            if (levels[nextStageKey].isUnlocked)
            {
                nextButton.interactable = true;
                isNextStage = true;
                nextButton.onClick.AddListener(changeGameSceneState);
            }
        }
    }
    void InitializeLevelSelectButton()
    {
        // change the state of the level selection stage
        // currently unavailable
    }

}