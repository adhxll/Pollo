using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalController : MonoBehaviour
{
    public GameObject modal;
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI scoreText;
    public string levelValue;
    public string scoreValue;
    public GameObject overlay;
 
    public void SetValues()
    {
        levelText.text = levelValue;
        scoreText.text = scoreValue;
    }
    public void ShowLevelModal(GameObject sourceLevel)
    {
        //set modal data menjadi level data
        var levelData = sourceLevel.GetComponent<LevelItem>().data;
        modal.GetComponent<StarCounter>().StarCount = levelData.starCount;
        this.scoreValue = levelData.highScore.ToString();
        this.levelValue = "Level " + levelData.getLevelCount();
        SetLevelToPlay(int.Parse(levelData.getLevelCount())); 
        //TODO: - get level ID then set modal data menjadi level ID

        if (!modal.activeSelf)
        {
            modal.GetComponent<StarCounter>().FillStars();
            this.SetValues();
            AnimationUtilities.AnimatePopUp(modal);
            modal.SetActive(true);
            overlay.SetActive(true);
        }
    }
    public void SetLevelToPlay(int selectedLevel)
    {
        var controller = GameController.instance;
        controller.selectedLevel = selectedLevel;
    }

    public void SetSceneToPlay(string scene)
    {
        GameController.instance.sceneState = (SceneStateManager.SceneState)System.Enum.Parse(typeof(SceneStateManager.SceneState), scene);
    }

    public void CloseModal()
    {
        AnimationUtilities.AnimatePopUpDisappear(modal);
        Invoke(nameof(DeactivateModal), 0.2f);  //biar animasinya keplay dulu sebelom diclose
    }
    private void DeactivateModal()
    {
        modal.GetComponent<StarCounter>().EmptyStars();
        modal.SetActive(false);
        overlay.SetActive(false);
    }

}
