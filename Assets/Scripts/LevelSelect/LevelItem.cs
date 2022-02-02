using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    // Data to fill the level item
    public LevelItemContainer data;
    private bool isUnlocked;

    // Containers for showing the data
    public TMPro.TextMeshPro levelCountText; 
    public GameObject starContainer;
    public GameObject lockUnlockCircle;
    public Sprite unlockedCircleSprite;
    public GameObject lockImage; 
    public Level levelSO;

    void Start()
    {
        //setup the data into container
        isUnlocked = data.isUnlocked; 
        levelCountText.GetComponent<TMPro.TextMeshPro>().text = data.levelID.ToString(); 
        starContainer.GetComponent<StarCounter>().starCount = data.starCount;
        starContainer.GetComponent<StarCounter>().FillStars();
        if (isUnlocked && data.sessionCount > 0) lockUnlockCircle.GetComponent<SpriteRenderer>().sprite = unlockedCircleSprite;
        var levelList = DataController.Instance.levelDatabase.allLevels; 
        for (int i = 1; i < levelList.Count; i++) { //start from after onboarding
            if (levelList[i].GetLevelID() == data.levelID && levelList[i].GetStageID() == GameController.Instance.currentStage) {
                
                levelSO = levelList[i];
                levelCountText.gameObject.SetActive(data.isUnlocked);
                starContainer.SetActive(data.isUnlocked);
                lockImage.SetActive(!data.isUnlocked);
                //Debug.Log("levelSO stage and level: " + levelSO.GetStageID() + " " + levelSO.GetLevelID());
            }
        }
    }

    public void Push() {
        if (isUnlocked)
        {
            PerspectivePan.SetPanning();
            AnimationUtilities.AnimateButtonPush(gameObject);
            ModalController.Instance.ShowLevelModal(gameObject);
        }
        else {
            if(PlayerPrefs.GetInt("DevMode") == 1) ModalController.Instance.ShowDeveloperModal(gameObject);
        }
    }
}
