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
    public TMPro.TextMeshPro LevelCountText; 
    public GameObject StarContainer;
    public GameObject LockUnlockCircle;
    public Sprite UnlockedCircleSprite;
    public GameObject lockImage; 
    public Level levelSO;

    void Start()
    {
        //setup the data into container
        isUnlocked = data.isUnlocked; 
        LevelCountText.GetComponent<TMPro.TextMeshPro>().text = data.levelID.ToString(); 
        StarContainer.GetComponent<StarCounter>().StarCount = data.starCount;
        StarContainer.GetComponent<StarCounter>().FillStars();
        if (isUnlocked && data.finishCount > 0) LockUnlockCircle.GetComponent<SpriteRenderer>().sprite = UnlockedCircleSprite;
        var levelList = DataController.Instance.levelDatabase.allLevels; 
        for (int i = 1; i < levelList.Count; i++) { //start from after onboarding
            if (levelList[i].GetLevelID() == data.levelID && levelList[i].GetStageID() == GameController.Instance.currentStage) {
                
                levelSO = levelList[i];
                LevelCountText.gameObject.SetActive(data.isUnlocked);
                StarContainer.SetActive(data.isUnlocked);
                lockImage.SetActive(!data.isUnlocked);
                //Debug.Log("levelSO stage and level: " + levelSO.GetStageID() + " " + levelSO.GetLevelID());
            }
        }
    }

    public void Push() {
        if (isUnlocked)
        {
            AnimationUtilities.AnimateButtonPush(gameObject);
            ModalController.Instance.ShowLevelModal(gameObject);
        }
    }
}
