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
        this.levelValue = "Level " + levelData.levelID; 
        SetLevelToPlay(levelData.levelID); 
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

}
