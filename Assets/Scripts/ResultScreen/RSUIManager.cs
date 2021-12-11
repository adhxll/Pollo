using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RSUIManager : MonoBehaviour
{
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button levelSelectButton = null;

    private bool isNextLevel = false;
    private bool isNextStage = false;

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
        else if (isNextStage)
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


