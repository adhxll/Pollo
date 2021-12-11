using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RSUIManager : MonoBehaviour
{
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button levelSelectButton = null;

    private void Start()
    {
        InitializeNextButton();
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
                GameController.Instance.selectedLevel = +1;
            }

        }
        else if (levels.ContainsKey(nextStageKey)) //if the next stage is available
        {
            Debug.Log("ada " + nextStageKey);
            if (levels[nextStageKey].isUnlocked)
            {
                nextButton.interactable = true;
                GameController.Instance.selectedLevel = 1;
                GameController.Instance.currentStage += 1;
            }
        }
    }

    // We need an indicator of level and stage in GameScene
    void GoToNextLevel()
    {
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene);
    }
}


