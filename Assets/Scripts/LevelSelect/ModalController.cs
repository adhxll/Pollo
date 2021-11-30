using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalController : MonoBehaviour
{
    public static ModalController Instance; 
    public GameObject modal;
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI accuracyText; 
    public string levelValue;
    public string scoreValue;
    public string accuracyValue; 
    public GameObject overlay;
    private void Awake()
    {
        StartCustomSingleton(); 
    }
    public void SetValues()
    {
        levelText.text = levelValue;
        scoreText.text = scoreValue;
        accuracyText.text = accuracyValue; 
    }
    public void ShowLevelModal(GameObject sourceLevel)
    {
        //set modal data menjadi level data
        var levelData = sourceLevel.GetComponent<LevelItem>().data;
        modal.GetComponent<StarCounter>().StarCount = levelData.starCount;
        Instance.scoreValue = levelData.highScore.ToString();
        Instance.accuracyValue = levelData.accuracy.ToString(); 
        Instance.levelValue = "Level " + levelData.levelID; 
        SetLevelToPlay(levelData.levelID);

        if (!modal.activeSelf)
        {
            modal.GetComponent<StarCounter>().FillStars();
            Instance.SetValues();
            AnimationUtilities.AnimatePopUp(modal);
            modal.SetActive(true);
            overlay.SetActive(true);
        }
    }
    private void SetLevelToPlay(int selectedLevel)
    {
        var controller = GameController.Instance;
        controller.selectedLevel = selectedLevel;
    }

    public void SetSceneToPlay(string scene)
    {
        GameController.Instance.sceneState = (SceneStateManager.SceneState)System.Enum.Parse(typeof(SceneStateManager.SceneState), scene);
    }
    public void CloseModal()
    {
        AnimationUtilities.AnimatePopUpDisappear(modal);
        StartCoroutine(DeactivateModal(0.2f));   //biar animasinya keplay dulu sebelom diclose
    }
    private IEnumerator DeactivateModal(float countTime)
    {
        yield return new WaitForSeconds(countTime); 
        modal.GetComponent<StarCounter>().EmptyStars();
        modal.SetActive(false);
        overlay.SetActive(false);
        StopCoroutine(DeactivateModal(0.2f)); 
    }
    void StartCustomSingleton() {
        // used to keep this script as a static singleton, but only within the level selection scene
        // needed for supporting multiple island prefabs.
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

}
