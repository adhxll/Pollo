using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalController : MonoBehaviour
{
    public static ModalController Instance;

    //public GameObject modal;
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI accuracyText; 
    public string levelValue;
    public string scoreValue;
    public string accuracyValue;
    public int starCount;
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
        var levelData = sourceLevel.GetComponent<LevelItem>().data;
        SetLevelToPlay(levelData.levelID);

        starCount = levelData.starCount;
        scoreValue = levelData.highScore.ToString();
        accuracyValue = levelData.accuracy.ToString();
        levelValue = $"Level {levelData.levelID}";

        AudioController.Instance.PlayButtonSound();
        SceneManagerScript.Instance.SceneInvoke(SceneManagerScript.SceneName.LSModal, true);
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
        //AnimationUtilities.AnimatePopUpDisappear(modal);
        PerspectivePan.SetPanning(); 
        StartCoroutine(DeactivateModal(0.2f));   //biar animasinya keplay dulu sebelom diclose
    }

    private IEnumerator DeactivateModal(float countTime)
    {
        yield return new WaitForSeconds(countTime); 
        overlay.SetActive(false);
        StopCoroutine(DeactivateModal(0.2f)); 
    }

    void StartCustomSingleton()
    {
        // Used to keep this script as a static singleton, but only within the level selection scene
        // Needed for supporting multiple island prefabs.
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

}
