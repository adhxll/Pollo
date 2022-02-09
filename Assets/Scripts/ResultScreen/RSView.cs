using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// View Class for ResultScreem
// Replacement of RSUIManager
public class RSView : RSElements
{
    [SerializeField] private TMP_Text scoreMessageObject = null;
    [SerializeField] private TMP_Text scoreObject = null; // the Score game object on ResultPage scene
    [SerializeField] private TMP_Text accuracyObject = null; // the Score game object on ResultPage scene
    [SerializeField] private GameObject[] stars = null; // the yellow stars inside the Tag GameObject
    [SerializeField] private GameObject levelSelectButton = null;
    [SerializeField] private GameObject restartButton = null;
    [SerializeField] private GameObject settingsButton = null;
    [SerializeField] private Button nextButton = null;
    private bool isNextLevel = false;
    private bool isNextStage = false;

    // Start is called before the first frame update
    void Awake()
    {
        SetScoreText();
        SetAccuracyText();
        SetStarIndicator();
        SetSuccessMessage();
    }

    void Start(){
        InitializeNextButtonState();
    }

    void SetScoreText()
    {
        scoreObject.text = app.model.GetScore().ToString();
    }

    void SetAccuracyText()
    {
        accuracyObject.text = app.model.GetAccuracy().ToString() + "%";
    }

    void SetStarIndicator()
    {
        StartCoroutine(StarAnimation());
    }

    // Depends on the star, it will show a corresponding message
    void SetSuccessMessage()
    {
        scoreMessageObject.text = app.model.GetSuccessMessage();
    }

    IEnumerator StarAnimation(){
        // Need to add animation
        int star = app.model.GetStar();
        yield return new WaitForSeconds(1f);
        if (star > 0) {
            stars[0].GetComponent<Image>().enabled = true;
            AnimationUtilities.Instance.PunchScale(stars[0]);
            AudioController.Instance.PlaySound(SoundNames.star1);
        }
        yield return new WaitForSeconds(1f);
        if (star > 1) {
            stars[1].GetComponent<Image>().enabled = true;
            AnimationUtilities.Instance.PunchScale(stars[1]);
            AudioController.Instance.PlaySound(SoundNames.star2);
        }
        yield return new WaitForSeconds(1f);
        if (star > 2) {
            stars[2].GetComponent<Image>().enabled = true;
            AnimationUtilities.Instance.PunchScale(stars[2]);
            AudioController.Instance.PlaySound(SoundNames.star3);
        }
    }

    public void OnClickedLevelSelectButton()
    {
        ButtonController.OnButtonClick(levelSelectButton);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LevelSelection);
    }

    public void OnClickedRestartButton()
    {
        ButtonController.OnButtonClick(restartButton);       
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene);
    }

    public void OnClickedNextButton(){
        ButtonController.OnButtonClick(nextButton.gameObject);
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.GameScene); 
    }

    public void OnClickedSettingsButton(){
        ButtonController.OnButtonClick(settingsButton);
    }

    void ChangeGameSceneLevel()
    {
        PlayerPrefs.SetInt("IsFirstTime",1);
        if (isNextLevel)
        {
            GameController.Instance.selectedLevel += 1;
        }
        else if (isNextStage)
        {
            GameController.Instance.selectedLevel = 1; //first level at every stage
            GameController.Instance.currentStage += 1;
        }
    }

    void InitializeNextButtonState()
    {
        isNextLevel = false;
        isNextStage = false;
        nextButton.interactable = false;
        // next level dict key
        string nextLevelKey = DataController.Instance.FormatKey(GameController.Instance.currentStage, GameController.Instance.selectedLevel + 1);
        // next stage dict key
        string nextStageKey = DataController.Instance.FormatKey(GameController.Instance.currentStage + 1, 1);

        Dictionary<string, LevelItemContainer> levels = DataController.Instance.playerData.levelData;

        if (levels.ContainsKey(nextLevelKey))
        {
            if (levels[nextLevelKey].isUnlocked)
            {
                nextButton.interactable = true;
                isNextLevel = true;
                nextButton.onClick.AddListener(ChangeGameSceneLevel);
            }
        }
        else if (levels.ContainsKey(nextStageKey)) //if the next stage is available
        {
            if (levels[nextStageKey].isUnlocked)
            {
                nextButton.interactable = true;
                isNextStage = true;
                nextButton.onClick.AddListener(ChangeGameSceneLevel);
            }
        }
    }

}
